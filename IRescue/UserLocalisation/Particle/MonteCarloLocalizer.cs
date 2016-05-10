using MathNet.Numerics.Distributions;
using UserLocalisation.PositionPrediction;

namespace IRescue.UserLocalisation.Particle
{
    using Sensors;
    using Core.DataTypes;
    using System;
    using System.Collections.Generic;

    public class MonteCarloLocalizer : AbstractUserLocalizer, IOrientationReceiver, IPositionReceiver
    {
        private int particleAmount;

        private Vector3 locationgrid;

        private Vector3 orientationgrid;

        private long currentTimeStamp;

        public List<MonteCarloParticle> Particlelist { get; set; }

        private List<IPositionSource> posources;

        private List<IOrientationSource> orisources;

        private LinearPredicter posePredicter;

        public MonteCarloLocalizer(int particleAmount, Vector3 locationgrid, Vector3 orientationgrid)
        {
            this.posources = new List<IPositionSource>();
            this.orisources = new List<IOrientationSource>();
            this.posePredicter = new LinearPredicter();
            this.particleAmount = particleAmount;
            this.locationgrid = locationgrid;
            this.orientationgrid = orientationgrid;
            this.Particlelist = this.InitParticles();
            this.WeighParticles();
        }
        public override Pose CalculatePose(long timeStamp)
        {
            this.MoveParticles(posePredicter.predictPositionAt(timeStamp));
            this.currentTimeStamp = timeStamp;
            this.Resample();
            this.WeighParticles();
            Pose result = this.WeightedAverageParticles();
            this.posePredicter.addPose(result, timeStamp);
            return result;
        }

        public Pose WeightedAverageParticles()
        {
            var calculatedPose = new Pose();

            foreach (var particle in this.Particlelist)
            {
                calculatedPose.Position.Add(particle.pose.Position.Multiply(particle.Weight));
                calculatedPose.Orientation.Add(particle.pose.Orientation.Multiply(particle.Weight));
            }
            return calculatedPose;
        }

        private List<MonteCarloParticle> InitParticles()
        {
            //Maybe speedup if list is prelocated
            var particles = new List<MonteCarloParticle>();
            for (var i = 0; i < particleAmount; i++)
            {
                float percentage = i / particleAmount;
                var xyz = new Vector3(locationgrid.Values);
                xyz.Multiply(percentage);
                var pyr = new Vector3(orientationgrid.Values);
                pyr.Multiply(percentage);
                particles.Add(new MonteCarloParticle(xyz, pyr, percentage));
            }
            return particles;
        }

        public float[] cumsum()
        {
            var cumsum = new float[this.particleAmount];
            cumsum[0] = this.Particlelist[0].Weight;
            var i = 1;
            while (i < cumsum.Length)
            {
                cumsum[i] = cumsum[i - 1] + this.Particlelist[i].Weight;
                i++;
            }
            return cumsum;
        }

        public float[] linspace(float start, float end, int amount)
        {
            if (start > end)
            {
                throw new NotImplementedException();
            }
            var linspaced = new float[amount + 1];
            var step = (end - start) / amount;
            for (var i = 0; i < amount; i++)
            {
                linspaced[i] = start + step * i;
            }
            linspaced[amount] = 1;
            return linspaced;
        }

        public void Resample()
        {
            var cumsum = this.cumsum();
            var linspaced = this.linspace(0, 1 - 1 / this.particleAmount, this.particleAmount);
            var newparticlepointer = 0;
            var oldparticlepointer = 0;
            var newparlist = new List<MonteCarloParticle>();
            while (newparticlepointer < this.particleAmount)
            {
                if (linspaced[newparticlepointer] < cumsum[oldparticlepointer])
                {
                    newparlist.Add(this.Particlelist[oldparticlepointer].duplicate());
                    newparticlepointer++;
                }
                else
                {
                    oldparticlepointer++;
                }
            }
            this.Particlelist = newparlist;
        }

        public void WeighParticles()
        {
            float sum = 0;
            foreach (var particle in Particlelist)
            {
                particle.Weight = calculateProbabity(particle); ;
                sum += particle.Weight;
            }
            normalizeParticleWeights(sum);
        }

        public float calculateProbabity(MonteCarloParticle particle)
        {
            double p = 1;
            foreach (var source in this.posources)
            {
                Measurement<Vector3> measurem = source.GetPosition(this.currentTimeStamp);
                p = p * prob(particle.pose.Position, measurem);
            }
            foreach (var source in this.orisources)
            {
                Measurement<Vector3> measurem = source.GetOrientation(this.currentTimeStamp);
                p = p * prob(particle.pose.Orientation, measurem);
            }

            return (float)p;
        }

        private double prob(Vector3 xyz, Measurement<Vector3> meas)
        {
            double p = 1;
            for (int i = 0; i < xyz.Count; i++)
            {
                p = p * Normal.PDF(xyz.Values[i], meas.Std, meas.Data.Values[i]);

            }
            return p;
        }

        public void normalizeParticleWeights(float sum)
        {
            foreach (var particle in Particlelist)
            {
                particle.Weight = particle.Weight / sum;
            }
        }

        public void MoveParticles(Pose translation)
        {
            foreach (var particle in Particlelist)
            {
                particle.pose.Position.Add(translation.Position);
                particle.pose.Orientation.Add(translation.Orientation);
            }
        }

        public void AddOrientationSource(IOrientationSource source)
        {
            this.orisources.Add(source);
        }

        public void AddPositionSource(IPositionSource source)
        {
            this.posources.Add(source);
        }
    }
}
