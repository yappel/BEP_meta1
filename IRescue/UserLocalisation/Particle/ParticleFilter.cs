// <copyright file="ParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle.Algos;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Sensors;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.Random;
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
        private const double NOISESIZE = 0.1;
        private List<IOrientationSource> orilist;
        private List<IPositionSource> poslist;
        private readonly double probabilityMargin;
        private long previousTS = 0;
        private LinearPredicter posePredictor;
        private double[] maxima;
        private AbstractParticleGenerator particlegen;



        public ParticleFilter(double[] maxima, int particleamount, double probabilityMargin)
        {
            this.particlegen = new RandomGenerator(new SystemRandomSource());
            this.posePredictor = new LinearPredicter();
            this.probabilityMargin = probabilityMargin;
            this.maxima = maxima;
            particles = new DenseMatrix(particleamount, DIMENSIONSAMOUNT,
                this.particlegen.Generate(particleamount, DIMENSIONSAMOUNT, maxima));
            float[] initweights = new float[particleamount * DIMENSIONSAMOUNT];
            ParticleFilter.FillArray<float>(initweights, 1f);
            weights = new DenseMatrix(particleamount, DIMENSIONSAMOUNT, initweights);
            normalizeWeightsAll(weights);
            poslist = new List<IPositionSource>();
            orilist = new List<IOrientationSource>();
        }


        public override Pose CalculatePose(long timeStamp)
        {
            RetrieveMeasurements(timeStamp);
            resample();
            predict(timeStamp);
            update();
            previousTS = timeStamp;
            return getResult(timeStamp);
        }

        private void resample()
        {
            Resample.Multinomial(this.particles, this.weights);
            NoiseGenerator.Uniform(this.particles, NOISESIZE);
            ContainParticles(this.particles, this.maxima);
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
                AddWeights(this.probabilityMargin, particles.SubMatrix(0, particles.RowCount, 0, 3), measurementspos, weights);
            }
            if (measurementsori != null)
            {
                AddWeights(this.probabilityMargin, particles.SubMatrix(0, particles.RowCount, 3, 3), measurementsori, weights.SubMatrix(0, particles.RowCount, 3, 3));
            }
            normalizeWeightsAll(this.weights);
        }

        private Pose getResult(long timeStamp)
        {
            float[] averages = WeightedAverage(this.particles, this.weights);
            Pose result = new Pose(new Vector3(averages[0], averages[1], averages[2]),
                new Vector3(averages[3], averages[4], averages[5]));
            posePredictor.addPose(result, timeStamp);
            return result;
        }

        public void ContainParticles(Matrix<float> matrix, double[] doubles)
        {
            for (int i = 0; i < matrix.ColumnCount / 2; i++)
            {
                Vector<float> res = matrix.Column(i).Map(c =>
                {
                    if (c > doubles[i])
                    {
                        return (float)doubles[i];
                    }
                    else if (c < 0)
                    {
                        return 0f;
                    }
                    else
                    {
                        return c;
                    }
                });
                matrix.SetColumn(i, res.ToArray());
            }
        }

        public void AddOrientationSource(IOrientationSource source)
        {
            orilist.Add(source);
        }

        public void AddPositionSource(IPositionSource source)
        {
            poslist.Add(source);
        }

        public float[] WeightedAverage(Matrix<float> particles, Matrix<float> weights)
        {
            Matrix<float> particlesdupe = particles.Clone();
            particles.PointwiseMultiply(weights, particlesdupe);
            return particlesdupe.ColumnSums().ToArray();
        }

        public void normalizeWeightsAll(Matrix<float> weights)
        {
            int columncount = 0;
            foreach (Vector<float> dimension in weights.EnumerateColumns())
            {
                normalizeWeights(dimension);
                weights.SetColumn(columncount, dimension);
                columncount++;
            }
        }

        public void normalizeWeights(Vector<float> list)
        {
            float sum = list.Sum();
            list.Divide(sum, list);
        }

        public void RetrieveMeasurements(long timeStamp)
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

            if (std.Count() == 0)
            {
                this.measurementspos = null;
            }
            else
            {
                this.measurementspos = new DenseMatrix(std.Count, 4, measx.ToArray());
            }

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

        public void AddWeights(double margin, Matrix<float> particles, Matrix<float> measurements, Matrix<float> weights)
        {
            for (int i = 0; i < particles.ColumnCount; i++)
            {
                for (int index = 0; index < particles.Column(i).Count; index++)
                {
                    float particle = particles.Column(i)[index];
                    var p = 1d;
                    for (int j = 0; j < measurements.Column(i).Count; j++)
                    {
                        float std = measurements[j, 3];
                        float meas = measurements[j, i];
                        p = p * (Normal.CDF(particle, std, meas + margin) -
                                 Normal.CDF(particle, std, meas - margin));
                    }
                    weights[index, i] = (float)p;
                }
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
