// <copyright file="ParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>


using System;
using System.Text;

namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;
    using System.Linq;
    using Algos.NoiseGenerators;
    using Algos.ParticleGenerators;
    using Algos.Resamplers;
    using Core.DataTypes;
    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using PosePrediction;
    using Sensors;

    /// <summary>
    /// Filters the measurements of orientation and position sources to determine a <see cref="Pose"/>. It uses a technique with Particles to determine this pose.
    /// </summary>
    public class ParticleFilter : AbstractUserLocalizer, IOrientationReceiver, IPositionReceiver
    {
        /// <summary>
        /// The amount of dimensions. Currently position{x,y,z} and orientation{x,y,z}
        /// </summary>
        private const int DIMENSIONSAMOUNT = 6;

        /// <summary>
        /// The amount of degrees in a circle.
        /// </summary>
        private const float ORIENTATIONMAX = 360;

        /// <summary>
        /// List containing the added <see cref="IOrientationSource"/>s
        /// </summary>
        private List<IOrientationSource> orilist;

        /// <summary>
        /// List containing the added <see cref="IPositionSource"/>s
        /// </summary>
        private List<IPositionSource> poslist;

        /// <summary>
        /// The amount of margin that will be used in on direction by calculating the Weights of the Particles using the normal cumulative density function
        /// </summary>
        private double probabilityMargin;

        /// <summary>
        /// The Stopwatch timestamp of the previous time a <see cref="Pose"/> was estimated
        /// </summary>
        private long previousTS = 0;

        /// <summary>
        /// The maximum amount that can be added or subtracted from the value of a particle by the <see cref="INoiseGenerator"/>.
        /// </summary>
        private float noisesize;

        private float[] ranges;
        private float[] minima;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleFilter"/> class.
        /// </summary>
        /// <param name="fieldsize">The maximum and minimum values of the positions</param>
        /// <param name="particleamount">The amount of particles to generate in each dimension</param>
        /// <param name="probabilityMargin">The amount the X in the normal CDF calculation will be moved in both directions to calculate p(X)</param>
        /// <param name="noisesize"> The maximum amount that can be added or subtracted from the value of a particle by the <see cref="INoiseGenerator"/>.</param>
        /// <param name="prtclgen">The particle generator used to generate particles</param>
        /// <param name="posePredictor">The class that predicts the next Pose, which is used to move the particles</param>
        /// <param name="noisegen">The noise generator the generate the noise that is added to the particles</param>
        /// <param name="resampler">The resample class that eliminates the particles with small weights</param>
        public ParticleFilter(
            FieldSize fieldsize,
            int particleamount,
            double probabilityMargin,
            float noisesize,
            IParticleGenerator prtclgen,
            IPosePredictor posePredictor,
            INoiseGenerator noisegen,
            IResampler resampler) : base(fieldsize)
        {
            this.poslist = new List<IPositionSource>();
            this.orilist = new List<IOrientationSource>();
            this.Particlegen = prtclgen;
            this.PosePredictor = posePredictor;
            this.Resampler = resampler;
            this.Noisegen = noisegen;
            this.probabilityMargin = probabilityMargin;
            this.noisesize = noisesize;
            generateFreshParticles(particleamount);
            this.ranges = new float[]
            {
                this.Fieldsize.Xmax - this.Fieldsize.Xmin, this.Fieldsize.Ymax - this.Fieldsize.Ymin,
                this.Fieldsize.Zmax - this.Fieldsize.Zmin,  ORIENTATIONMAX,  ORIENTATIONMAX, ORIENTATIONMAX
            }; this.minima = new float[]
             {
                this.Fieldsize.Xmin,this.Fieldsize.Ymin, this.Fieldsize.Zmin, 0, 0, 0
             };
        }

        private void generateFreshParticles(int particleamount)
        {
            // Create particle matrix
            var particlearray = this.Particlegen.Generate(
                particleamount,
                DIMENSIONSAMOUNT);
            this.Particles = new DenseMatrix(particleamount, DIMENSIONSAMOUNT, particlearray);

            // Create weights matrix
            float[] initweights = new float[particleamount * DIMENSIONSAMOUNT];
            FillArray(initweights, 1f / particleamount);
            this.Weights = new DenseMatrix(particleamount, DIMENSIONSAMOUNT, initweights);
        }

        /// <summary>
        /// Gets or sets a matrix containing the values of the Particles for every dimension. The values are stored in rows and there is one column for every dimension.
        /// </summary>
        public Matrix<float> Particles { get; set; }

        /// <summary>
        /// Gets or sets a matrix containing the Weights of the Particles for every dimension. The Weights are stored in rows and there is one column for every dimension.
        /// </summary>
        public Matrix<float> Weights { get; set; }

        /// <summary>
        /// Gets or sets list containing all the measurements from the <see cref="IOrientationSource"/>s
        /// </summary>
        public Matrix<float> Measurementsori { get; set; }

        /// <summary>
        /// Gets or sets list containing all the measurements from the <see cref="IPositionSource"/>s
        /// </summary>
        public Matrix<float> Measurementspos { get; set; }

        /// <summary>
        /// Gets or sets the generator that is used generate a new set of Particles 
        /// </summary>
        private IParticleGenerator Particlegen { get; set; }

        /// <summary>
        /// Gets or sets the class that is used to Predict the next <see cref="Pose"/>
        /// </summary>
        private IPosePredictor PosePredictor { get; set; }

        /// <summary>
        /// Gets or sets the generator that generates the noise
        /// </summary>
        private INoiseGenerator Noisegen { get; set; }

        /// <summary>
        /// Gets or sets the class that resamples the particles
        /// </summary>
        private IResampler Resampler { get; set; }

        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        /// <returns>An educated guess of the pose of measured object/person</returns>
        public override Pose CalculatePose(long timeStamp)
        {
            //testprint();
            this.RetrieveMeasurements(timeStamp);
            this.Resample();
            //testprint();
            this.Predict(timeStamp);
            //testprint();
            this.Update();
            this.previousTS = timeStamp;
            Pose res = this.GetResult(timeStamp);
            //testprint();
            return res;
        }

        /// <summary>
        /// Adds a <see cref="IOrientationSource"/> to the list of sources.
        /// </summary>
        /// <param name="source">The source to add</param>
        public void AddOrientationSource(IOrientationSource source)
        {
            this.orilist.Add(source);
        }

        /// <summary>
        /// Adds a <see cref="IPositionSource"/> to the list of sources.
        /// </summary>
        /// <param name="source">The source to add</param>
        public void AddPositionSource(IPositionSource source)
        {
            this.poslist.Add(source);
        }

        /// <summary>
        /// Maps all values in the matrix that are bigger than the maximum to the maximum and the values that are lower than 0 to 0;
        /// </summary>
        /// <param name="matrix">The matrix containing the values to perform the action on.</param>
        public void ContainParticles(Matrix<float> matrix)
        {
            this.ContainParticles(matrix, 0, this.Fieldsize.Xmin, this.Fieldsize.Xmax);
            this.ContainParticles(matrix, 1, this.Fieldsize.Ymin, this.Fieldsize.Ymax);
            this.ContainParticles(matrix, 2, this.Fieldsize.Zmin, this.Fieldsize.Zmax);
        }

        /// <summary>
        /// Calculates the weighted average of the Particles in a dimension
        /// </summary>
        /// <param name="particles">Matrix containing the values of the Particles. One column for every dimension.</param>
        /// <param name="weights">Matrix of the same size as <see cref="Particles"/> containing the Weights.</param>
        /// <returns>An array with a length equal to the amount of columns of <see cref="Particles"/> containing the weighted average of the corresponding column</returns>
        public float[] WeightedAverage(Matrix<float> particles, Matrix<float> weights)
        {
            Matrix<float> particlesdupe = particles.Clone();
            Matrix<float> weightsdupe = weights.Clone();
            particles.PointwiseMultiply(weightsdupe, particlesdupe);
            return particlesdupe.ColumnSums().ToArray();
        }

        /// <summary>
        /// Normalize the values in every column of a matrix.
        /// </summary>
        /// <param name="weights">The matrix to normalize the Weights in every column in</param>
        public void NormalizeWeightsAll(Matrix<float> weights)
        {
            int columncount = 0;
            foreach (Vector<float> dimension in weights.EnumerateColumns())
            {
                if (Math.Abs(dimension.Sum()) < 0.000000000000000000001)
                {
                    generateFreshParticles(dimension.ToArray().Length);
                }
                this.NormalizeWeights(dimension);
                weights.SetColumn(columncount, dimension);
                columncount++;
            }
        }

        /// <summary>
        /// Normalizes the values of a vector. This is not the same as normalizing the vector itself.
        /// </summary>
        /// <param name="list">The vector of which the values will be normalized</param>
        public void NormalizeWeights(Vector<float> list)
        {
            float sum = list.Sum();
            list.Divide(sum, list);
        }

        /// <summary>
        /// Retrieve the <see cref="Measurement{T}"/> of the <see cref="IOrientationSource"/>s and <see cref="IPositionSource"/>s
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public void RetrieveMeasurements(long timeStamp)
        {
            this.RetrievePosMeasurements(timeStamp);
            this.RetrieveOriMeasurements(timeStamp);
        }

        /// <summary>
        /// Generate a new Weights for the Particles for the position dimensions, based on the likelihood that the particle value is the correct value.
        /// </summary>
        /// <param name="margin">The amount the X in the normal CDF calculation will be moved in both directions to calculate p(X)</param>
        /// <param name="particles">The matrix containing the values of the Particles</param>
        /// <param name="measurements">The matrix containing the measurements of the <see cref="IPositionSource"/>s</param>
        /// <param name="weights">The matrix containing the current Weights of the Particles</param>
        public void AddWeightsPos(double margin, Matrix<float> particles, Matrix<float> measurements, Matrix<float> weights)
        {
            if (measurements == null)
            {
                return;
            }

            this.AddWeights(margin, particles, 0, 2, measurements, weights);
        }

        /// <summary>
        /// Generate a new Weights for the Particles for the orientation dimensions, based on the likelihood that the particle value is the correct value.
        /// </summary>
        /// <param name="margin">The amount the X in the normal CDF calculation will be moved in both directions to calculate p(X)</param>
        /// <param name="particles">The matrix containing the values of the Particles</param>
        /// <param name="measurements">The matrix containing the measurements of the <see cref="IOrientationSource"/>s</param>
        /// <param name="weights">The matrix containing the current Weights of the Particles</param>
        public void AddWeightsOri(double margin, Matrix<float> particles, Matrix<float> measurements, Matrix<float> weights)
        {
            if (measurements == null)
            {
                return;
            }

            for (int c = 0; c < measurements.ColumnCount - 1; c++)
            {
                for (int r = 0; r < measurements.RowCount; r++)
                {
                    measurements[r, c] = measurements[r, c] % 360;
                }
            }

            for (int c = 3; c < particles.ColumnCount; c++)
            {
                for (int r = 0; r < particles.RowCount; r++)
                {
                    particles[r, c] = particles[r, c] % 360;
                }
            }

            this.AddWeights(margin, particles, 3, 5, measurements, weights);
        }

        /// <summary>
        ///  Generate a new Weights for the Particles for the dimensions in columns <see cref="colfrom"/> to <see cref="colto"/>, based on the likelihood that the particle value is the correct value.
        /// </summary>
        /// <param name="margin">The amount the X in the normal CDF calculation will be moved in both directions to calculate p(X)</param>
        /// <param name="particles">The matrix containing the values of the Particles</param>
        /// <param name="colfrom">The lower bound of the range of columns to calculate new Weights for</param>
        /// <param name="colto">The upper bound of the range of columns to calculate new Weights for</param>
        /// <param name="measurements">The matrix containing the measurements of the <see cref="IOrientationSource"/>s</param>
        /// <param name="weights">The matrix containing the current Weights of the Particles</param>
        public void AddWeights(double margin, Matrix<float> particles, int colfrom, int colto, Matrix<float> measurements, Matrix<float> weights)
        {
            for (int i = colfrom; i < colto; i++)
            {
                for (int index = 0; index < particles.Column(i).Count; index++)
                {
                    float particle = particles.Column(i)[index] * this.ranges[i] + this.minima[i];
                    var p = 1d;
                    for (int j = 0; j < measurements.Column(colto - colfrom).Count; j++)
                    {
                        float std = measurements[j, 3];
                        float meas = measurements[j, i - colfrom];
                        p = p * (Normal.CDF(particle, std, meas + margin) -
                               Normal.CDF(particle, std, meas - margin));
                    }

                    weights[index, i] = (float)p;
                }
            }
        }

        /// <summary>
        /// Fills the array <see cref="arr"/> with values <see cref="value"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements for the array</typeparam>
        /// <param name="arr">The array to fill</param>
        /// <param name="value">The value to add to the array</param>
        private static void FillArray<T>(T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        /// <summary>
        /// Makes sure all values are between the minimum and maximum values.
        /// </summary>
        /// <param name="matrix">THe matrix to check the values from</param>
        /// <param name="columnindex">What column to check</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">THe maximum value</param>
        private void ContainParticles(Matrix<float> matrix, int columnindex, float min, float max)
        {
            Vector<float> particles = matrix.Column(columnindex);
            particles.Map(p =>
            {
                if (p > max)
                {
                    return max;
                }
                else if (p < 0)
                {
                    return min;
                }
                else
                {
                    return p;
                }
            });
            matrix.SetColumn(0, particles.ToArray());
        }

        /// <summary>
        /// Gets all the measurements from the orientation sources from last timestamp till current timestamp
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        private void RetrieveOriMeasurements(long timeStamp)
        {
            List<float> measx = new List<float>(),
                measy = new List<float>(),
                measz = new List<float>(),
                std = new List<float>();
            foreach (IOrientationSource orientationSource in this.orilist)
            {
                List<Measurement<Vector3>> measall = orientationSource.GetOrientations(this.previousTS, timeStamp);
                foreach (Measurement<Vector3> meas in measall)
                {
                    measx.Add(meas.Data.X);
                    measy.Add(meas.Data.Y);
                    measz.Add(meas.Data.Z);
                    std.Add(meas.Std);
                }
            }

            IEnumerable<float> concatenated = measx.Concat(measy).Concat(measz).Concat(std);
            this.Measurementsori = !std.Any() ? null : new DenseMatrix(std.Count, 4, concatenated.ToArray());
        }

        /// <summary>
        /// Gets all the measurements from the position sources from last timestamp till current timestamp
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        private void RetrievePosMeasurements(long timeStamp)
        {
            ////If speedup needed change dimensions of matrices to remove need of temp storage lists
            List<float> measx = new List<float>(),
                measy = new List<float>(),
                measz = new List<float>(),
                std = new List<float>();
            foreach (IPositionSource positionSource in this.poslist)
            {
                List<Measurement<Vector3>> measall = positionSource.GetPositions(this.previousTS, timeStamp);
                foreach (Measurement<Vector3> meas in measall)
                {
                    measx.Add(meas.Data.X);
                    measy.Add(meas.Data.Y);
                    measz.Add(meas.Data.Z);
                    std.Add(meas.Std);
                }
            }

            IEnumerable<float> concatenated = measx.Concat(measy).Concat(measz).Concat(std);
            this.Measurementspos = !std.Any() ? null : new DenseMatrix(std.Count, 4, concatenated.ToArray());
        }

        /// <summary>
        /// Generate a new collection of Particles based on the current collection of Particles and their Weights
        /// </summary>
        private void Resample()
        {
            this.Resampler.Resample(this.Particles, this.Weights);
            this.Noisegen.GenerateNoise(-1 * this.noisesize, this.noisesize, this.Particles);
            this.ContainParticles(this.Particles);
        }

        /// <summary>
        /// Predict the <see cref="Pose"/> of the current timestamp relative to the previous predictions and move the Particles accordingly
        /// </summary>
        /// <param name="timeStamp">the current timestamp</param>
        private void Predict(long timeStamp)
        {
            float[] translation = this.PosePredictor.PredictPoseAt(timeStamp);
            float[] transmatrixarray = new float[this.Particles.RowCount * this.Particles.ColumnCount];
            for (int c = 0; c < this.Particles.ColumnCount; c++)
            {
                for (int r = 0; r < this.Particles.RowCount; r++)
                {
                    if (c < 3)
                    {
                        transmatrixarray[(this.Particles.RowCount * c) + r] = translation[c] / this.ranges[c];
                    }
                    else
                    {
                        //TODO change modulo
                        transmatrixarray[(this.Particles.RowCount * c) + r] = (translation[c] % ORIENTATIONMAX) / ranges[c];
                    }
                }
            }

            Matrix<float> transmatrix = new DenseMatrix(this.Particles.RowCount, DIMENSIONSAMOUNT, transmatrixarray);
            this.Particles.Add(transmatrix, this.Particles);
        }

        /// <summary>
        /// Gives a weight based on the change that the value of the particle is the real value of the user and normalizes it for every column/dimension.
        /// </summary>
        private void Update()
        {
            this.AddWeightsPos(this.probabilityMargin, this.Particles, this.Measurementspos, this.Weights);
            this.AddWeightsOri(this.probabilityMargin, this.Particles, this.Measurementsori, this.Weights);
            this.NormalizeWeightsAll(this.Weights);
        }

        /// <summary>
        /// Calculates the weighted average of the Particles per dimension and creates a <see cref="Pose"/> based on that.
        /// </summary>
        /// <param name="timeStamp">current timestamp</param>
        /// <returns>Returns the estimated pose</returns>
        private Pose GetResult(long timeStamp)
        {
            float[] averages = this.WeightedAverage(this.Particles, this.Weights);
            Pose result = new Pose(
                new Vector3(averages[0] * this.ranges[0] - this.minima[0], averages[1] * this.ranges[1] - this.minima[1], averages[2] * this.ranges[2] - this.minima[2]),
                new Vector3(averages[3] * this.ranges[3] - this.minima[3], averages[4] * this.ranges[4] - this.minima[4], averages[5] * this.ranges[5] - this.minima[5]));
            this.PosePredictor.AddPoseData(timeStamp, result);
            return result;
        }

        private void testprint()
        {
            var particlepath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_particles.txt");
            System.Text.StringBuilder builder1 = new StringBuilder();
            for (int i = 0; i < this.Particles.RowCount; i++)
            {
                for (int jj = 0; jj < 6; jj++)
                {
                    builder1.Append(this.Particles[i, jj]);
                    builder1.Append(" ");
                    builder1.Append(this.Weights[i, jj]);
                    builder1.Append(" ");
                }
                builder1.AppendLine();
            }
            System.IO.File.AppendAllText(particlepath, builder1.ToString());
        }
    }
}