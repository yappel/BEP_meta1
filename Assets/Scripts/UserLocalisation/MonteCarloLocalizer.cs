// <copyright file="MonteCarloLocalizer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace Assets.Scripts.UserLocalisation
{
    using Assets.Scripts.Inputsensors;
    //using MathNet.Numerics.Distributions;
    //using MathNet.Numerics.LinearAlgebra;
    //using MathNet.Numerics.Random;
    using System;
    using System.Collections.Generic;
    class MonteCarloLocalizer : AbstractUserLocalizer, IMotionReceiver, ILocationReceiver
    {
        private int particleAmount;

        private IRVector3 locationgrid;

        private IRVector3 orientationgrid;

        private List<MonteCarloParticle> particlelist;

        private List<ILocationSource> locsources;

        private List<IMotionSource> motsources;

        public MonteCarloLocalizer(int particleAmount, IRVector3 locationgrid, IRVector3 orientationgrid)
        {
            this.particleAmount = particleAmount;
            this.locationgrid = locationgrid;
            this.orientationgrid = orientationgrid;
            this.particlelist = this.InitParticles();
            this.WeighParticles();
        }
        public override void CalculateLocation()
        {
            this.Resample();
            this.WeighParticles();
            this.position = this.WeightedAverageParticles();
        }

        public IRVector3 WeightedAverageParticles()
        {
            IRVector3 calculatedPosition = new IRVector3(0, 0, 0);
            foreach (MonteCarloParticle particle in particlelist)
            {
                calculatedPosition.Add(particle.Xyz.Multiply(particle.Weight));
            }
            return calculatedPosition;
        }

        public void RegisterLocationReceiver(ILocationSource source)
        {
            locsources.Add(source);
        }

        public void RegisterMotionSource(IMotionSource source)
        {
            motsources.Add(source);
        }

        private List<MonteCarloParticle> InitParticles()
        {
            //Maybe speedup if list is prelocated
            List<MonteCarloParticle> particlelist = new List<MonteCarloParticle>();
            for (int i = 0; i < this.particleAmount; i++)
            {
                float percentage = i / this.particleAmount;
                IRVector3 xyz = new IRVector3(locationgrid.GetX(), locationgrid.GetY(), locationgrid.GetZ());
                xyz.Multiply(percentage);
                IRVector3 pyr = new IRVector3(orientationgrid.GetX(), orientationgrid.GetY(), orientationgrid.GetZ());
                pyr.Multiply(percentage);
                particlelist.Add(new MonteCarloParticle(xyz, pyr, percentage));
            }
            return particlelist;
        }

        public float[] cumsum()
        {
            float[] cumsum = new float[this.particleAmount];
            cumsum[0] = this.particlelist[0].Weight;
            int i = 1;
            while (i < cumsum.Length)
            {
                cumsum[i] = cumsum[i - 1] + this.particlelist[i].Weight;
            }
            return cumsum;
        }

        public float[] linspace(float start, float end, int amount)
        {
            if (start > end)
            {
                throw new NotImplementedException();
            }
            float[] linspaced = new float[amount];
            float step = end - start;
            for (int i = 0; i < linspaced.Length; i++)
            {
                linspaced[i] = start + step * i;
            }
            return linspaced;
        }

        public void Resample()
        {
            Random rng = new Random();
            float[] cumsum = this.cumsum();
            float[] linspaced = this.linspace(0, 1 - 1 / this.particleAmount, this.particleAmount);
            int newparticlepointer = 1;
            int oldparticlepointer = 1;
            List<MonteCarloParticle> newparlist = new List<MonteCarloParticle>();
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
            foreach (MonteCarloParticle particle in particlelist)
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
            foreach (MonteCarloParticle particle in particlelist)
            {
                particle.Weight = particle.Weight / sum;
            }
        }

        public void MoveParticles(SensorVector3 translation)
        {
            throw new NotImplementedException();
        }
    }
}
