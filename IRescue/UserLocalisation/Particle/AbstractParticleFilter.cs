namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.PosePrediction;

    public abstract class AbstractParticleFilter
    {
        protected long currentTimeStamp;

        protected List<Measurement<Vector3>> measurements;

        protected long previousTimeStamp;

        private readonly double cdfmargin = 0.005;

        private readonly INoiseGenerator noiseGenerator;

        private readonly AbstractParticleController particleControllerX;

        private readonly AbstractParticleController particleControllerY;

        private readonly AbstractParticleController particleControllerZ;

        private float resampleNoiseSize;

        private readonly IResampler resampler;

        private ISmoother smoother;

        /// <summary>
        /// Function that calculates the average value given a list of values.
        /// </summary>
        private Func<float[], float> averageCalculator;

        private IExtrapolate iex = new FlexibleExtrapolate();
        private IExtrapolate iey = new FlexibleExtrapolate();
        private IExtrapolate iez = new FlexibleExtrapolate();

        protected AbstractParticleFilter(IResampler resampler, INoiseGenerator noiseGenerator, AbstractParticleController particleControllerX, AbstractParticleController particleControllerY, AbstractParticleController particleControllerZ, float resampleNoiseSize, ISmoother smoother, Func<float[], float> averageCalculator)
        {
            this.resampler = resampler;
            this.noiseGenerator = noiseGenerator;
            this.particleControllerX = particleControllerX;
            this.particleControllerY = particleControllerY;
            this.particleControllerZ = particleControllerZ;
            this.resampleNoiseSize = resampleNoiseSize;
            this.smoother = smoother;
            this.averageCalculator = averageCalculator;
            this.currentTimeStamp = -1;
            this.measurements = new List<Measurement<Vector3>>();
        }

        public Vector3 Calculate(long timeStamp)
        {
            this.previousTimeStamp = this.currentTimeStamp;
            this.currentTimeStamp = timeStamp;
            this.RetrieveMeasurements();
            this.Resample();
            //this.Predict();
            this.Update();
            return this.ProcessResults();
        }

        protected virtual Vector3 ProcessResults()
        {
            float resultX = this.ProcessResult(this.particleControllerX, this.iex);
            float resultY = this.ProcessResult(this.particleControllerY, this.iey);
            float resultZ = this.ProcessResult(this.particleControllerZ, this.iez);
            return this.smoother.GetSmoothedResult(new Vector3(resultX, resultY, resultZ), this.currentTimeStamp, this.averageCalculator);
        }

        private float ProcessResult(AbstractParticleController cont, IExtrapolate ie)
        {
            if (Math.Abs(cont.Weights.Sum()) < float.Epsilon)
            {
                this.noiseGenerator.GenerateNoise(0.01f, cont);
                cont.SetWeights(1f / cont.Count);
            }

            float result = this.GetWeightedAverage(cont);
            ie.AddData(this.currentTimeStamp, result);
            return result;
        }

        private float GetWeightedAverage(AbstractParticleController con)
        {
            con.NormalizeWeights();
            return con.WeightedAverage();
        }

        protected abstract void RetrieveMeasurements();

        private void CalculateWeights(AbstractParticleController parCon, IList<float> diffs, IDistribution dist)
        {
            for (int index = 0; index < diffs.Count; index++)
            {
                float diff = diffs[index];
                float p = (float)(dist.CDF(0, diff + this.cdfmargin) - dist.CDF(0, diff - this.cdfmargin));
                parCon.SetWeightAt(index, parCon.GetWeightAt(index) * p);
            }
        }

        private void Predict()
        {
            if (this.previousTimeStamp < 0)
            {
                return;
            }

            this.Predict(this.particleControllerX, this.iex);
            this.Predict(this.particleControllerY, this.iey);
            this.Predict(this.particleControllerZ, this.iez);
        }

        private void Predict(AbstractParticleController cont, IExtrapolate ie)
        {
            float prediction = (float)ie.PredictChange(this.previousTimeStamp, this.currentTimeStamp);
            cont.AddToValues(prediction);
        }

        private void Resample()
        {
            if (this.previousTimeStamp < 0)
            {
                return;
            }

            this.Resample(this.particleControllerX);
            this.Resample(this.particleControllerY);
            this.Resample(this.particleControllerZ);
        }

        private void Resample(AbstractParticleController particleController)
        {
            this.resampler.Resample(particleController);
            this.noiseGenerator.GenerateNoise(this.resampleNoiseSize, particleController);
        }

        private void Update()
        {
            this.particleControllerX.SetWeights(1f);
            this.particleControllerY.SetWeights(1f);
            this.particleControllerZ.SetWeights(1f);
            if (this.measurements != null)
            {
                for (int index = 0; index < this.measurements.Count; index++)
                {
                    Measurement<Vector3> measurement = this.measurements[index];
                    IDistribution dist = measurement.DistributionType;
                    float[] diffx = this.particleControllerX.DistanceToValue(measurement.Data.X);
                    float[] diffy = this.particleControllerY.DistanceToValue(measurement.Data.Y);
                    float[] diffz = this.particleControllerZ.DistanceToValue(measurement.Data.Z);
                    this.CalculateWeights(this.particleControllerX, diffx, dist);
                    this.CalculateWeights(this.particleControllerY, diffy, dist);
                    this.CalculateWeights(this.particleControllerZ, diffz, dist);
                }
            }
        }
    }
}