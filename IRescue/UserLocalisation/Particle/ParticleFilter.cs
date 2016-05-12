// <copyright file="ParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle.Algos;
using IRescue.UserLocalisation.Sensors;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using UserLocalisation.PositionPrediction;

namespace IRescue.UserLocalisation.Particle
{
    /// <summary>
    /// TODO
    /// </summary>
    public class ParticleFilter : AbstractUserLocalizer, IOrientationReceiver, IPositionReceiver
    {

        public Matrix<float> particles;
        public Matrix<float> weights;
        public Matrix<float> measurementsori;
        public Matrix<float> measurementspos;
        private const int DIMENSIONSAMOUNT = 6;
        private const int NOISESIZE = 5;
        private List<IOrientationSource> orilist;
        private List<IPositionSource> poslist;
        private readonly double probabilityMargin;
        private long previousTS = 0;
        private LinearPredicter posePredictor;



        public ParticleFilter(double[] maxima, int particleamount, double probabilityMargin)
        {
            this.posePredictor = new LinearPredicter();
            this.probabilityMargin = probabilityMargin;
            particles = new DenseMatrix(particleamount, DIMENSIONSAMOUNT,
                InitParticles.RandomUniform(particleamount, DIMENSIONSAMOUNT, maxima));
            float[] initweights = new float[particleamount * DIMENSIONSAMOUNT];
            ParticleFilter.FillArray<float>(initweights, 1f);
            weights = new DenseMatrix(particleamount, DIMENSIONSAMOUNT, initweights);
            poslist = new List<IPositionSource>();
            orilist = new List<IOrientationSource>();
        }


        public override Pose CalculatePose(long timeStamp)
        {
            AddMeasurements(timeStamp);
            previousTS = timeStamp;
            resample();
            predict(timeStamp);
            update();
            return getResult(timeStamp);
        }

        private void resample()
        {
            Resample.Multinomial(this.particles, this.weights);
            NoiseGenerator.Uniform(this.particles, NOISESIZE);
        }

        private void predict(long timeStamp)
        {
            float[] translation = posePredictor.predictPositionAt(timeStamp);
            Matrix<float> transmatrix = new DenseMatrix(1, DIMENSIONSAMOUNT, translation);
        }

        private void update()
        {
            if (measurementspos != null)
            {
                Feeder.AddWeights(this.probabilityMargin, particles.SubMatrix(0, particles.RowCount, 0, 3), measurementspos, weights.SubMatrix(0, particles.RowCount, 0, 3));
            }
            if (measurementsori != null)
            {
                Feeder.AddWeights(this.probabilityMargin, particles.SubMatrix(0, particles.RowCount, 3, 3), measurementsori, weights.SubMatrix(0, particles.RowCount, 3, 3));
            }
            normalizeWeightsAll(this.weights);
        }

        private Pose getResult(long timeStamp)
        {
            float[] averages = process(this.particles, this.weights);
            Pose result = new Pose(new Vector3(averages[0], averages[1], averages[2]),
                new Vector3(averages[3], averages[4], averages[5]));
            posePredictor.addPose(result, timeStamp);
            return result;
        }

        public void AddOrientationSource(IOrientationSource source)
        {
            orilist.Add(source);
        }

        public void AddPositionSource(IPositionSource source)
        {
            poslist.Add(source);
        }

        public float[] process(Matrix<float> particles, Matrix<float> weights)
        {
            particles.PointwiseMultiply(weights, particles);
            return particles.ColumnSums().ToArray();
        }

        public void normalizeWeightsAll(Matrix<float> weights)
        {
            foreach (Vector<float> dimension in weights.EnumerateColumns())
            {
                normalizeWeights(dimension);
            }
        }

        public void normalizeWeights(Vector<float> list)
        {
            float sum = list.Sum();
            list.Map(c => c / sum);
        }

        public void AddMeasurements(long timeStamp)
        {
            //If speedup needed change dimensions of matrices to remove need of temp storage lists
            var measx = new List<float>();
            var measy = new List<float>();
            var measz = new List<float>();
            var std = new List<float>();
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
            measx.AddRange(measy);
            measx.AddRange(measz);
            measx.AddRange(std);
            this.measurementspos = new DenseMatrix(std.Count, 4, measx.ToArray());

            measx.Clear();
            measy.Clear();
            measz.Clear();
            std.Clear();
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
            measx.AddRange(measy);
            measx.AddRange(measz);
            measx.AddRange(std);
            if (std.Count() == 0)
            {
                this.measurementsori = null;
            }
            else
            {
                this.measurementsori = new DenseMatrix(std.Count, 4, measx.ToArray());
            }

        }

        public static void FillArray<T>(T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
    }

}
