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

        private List<MonteCarloParticle> particlelist;

        private List<IPositionSource> locsources;

        private List<IOrientationSource> motsources;

        public MonteCarloLocalizer(int particleAmount, Vector3 locationgrid, Vector3 orientationgrid)
        {
            this.particleAmount = particleAmount;
            this.locationgrid = locationgrid;
            this.orientationgrid = orientationgrid;
            this.particlelist = this.InitParticles();
            this.WeighParticles();
        }
        public override Pose CalculatePose(long timeStamp)
        {
            this.Resample();
            this.WeighParticles();
            return this.WeightedAverageParticles();
        }

        public Pose WeightedAverageParticles()
        {
            var calculatedPose = new Pose();

            foreach (var particle in this.particlelist)
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
            cumsum[0] = this.particlelist[0].Weight;
            var i = 1;
            while (i < cumsum.Length)
            {
                cumsum[i] = cumsum[i - 1] + this.particlelist[i].Weight;
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
            var linspaced = new float[amount];
            var step = end - start;
            for (var i = 0; i < linspaced.Length; i++)
            {
                linspaced[i] = start + step * i;
            }
            return linspaced;
        }

        public void Resample()
        {
            var cumsum = this.cumsum();
            var linspaced = this.linspace(0, 1 - 1 / this.particleAmount, this.particleAmount);
            var newparticlepointer = 1;
            var oldparticlepointer = 1;
            var newparlist = new List<MonteCarloParticle>();
            while (newparticlepointer <= this.particleAmount)
            {
                if (linspaced[newparticlepointer] < cumsum[oldparticlepointer])
                {
                    newparlist.Add(this.particlelist[oldparticlepointer].duplicate());
                    newparticlepointer++;
                }
                else
                {
                    oldparticlepointer++;
                }
            }
            this.particlelist = newparlist;
        }

        public void WeighParticles()
        {
            float sum = 0;
            foreach (var particle in particlelist)
            {
                particle.Weight = calculateProbabity(particle); ;
                sum += particle.Weight;
            }
            normalizeParticleWeights(sum);
        }

        public float calculateProbabity(MonteCarloParticle particle)
        {
            return 1f;
            //Todo matrixes for multivariate normal distribution object
            //MatrixNormal normal = new MatrixNormal();
            //foreach (ILocationSource source in locsources)
            //{
            //    foreach(SensorVector3 measurement in source.GetLocations())
            //    {
            //        normal.Density(Matrix<double>.Build.Random(3, 4);)
            //    }
            //}
        }

        public void normalizeParticleWeights(float sum)
        {
            foreach (var particle in particlelist)
            {
                particle.Weight = particle.Weight / sum;
            }
        }

        public void MoveParticles(Vector3 translation)
        {
            throw new NotImplementedException();
        }

        public void AddOrientationSource(IOrientationSource source)
        {
            throw new NotImplementedException();
        }

        public void AddPositionSource(IPositionSource source)
        {
            throw new NotImplementedException();
        }
    }
}
