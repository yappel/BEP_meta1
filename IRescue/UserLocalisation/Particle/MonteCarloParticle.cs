// <copyright file="MonteCarloParticle.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
using System;

namespace Assets.Scripts.UserLocalisation
{
    /// <summary>
    /// TODO
    /// </summary>
    public class MonteCarloParticle
    {
        private IRVector3 xyz;
        private IRVector3 pyr;
        private float weight;

        public IRVector3 Xyz
        {
            get
            {
                return xyz;
            }

            set
            {
                xyz = value;
            }
        }

        public IRVector3 Pyr
        {
            get
            {
                return pyr;
            }

            set
            {
                pyr = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public MonteCarloParticle(IRVector3 xyz, IRVector3 pyr, float weight)
        {
            this.Xyz = xyz;
            this.Pyr = pyr;
            this.Weight = weight;
        }

        public void Translatexyz(IRVector3 translation)
        {
            Xyz.Add(translation);
        }

        public MonteCarloParticle duplicate()
        {
            IRVector3 newxyz = new IRVector3(this.xyz.GetX(), this.xyz.GetY(), this.xyz.GetZ());
            IRVector3 newpyr = new IRVector3(this.pyr.GetX(), this.pyr.GetY(), this.pyr.GetZ());
            return new MonteCarloParticle(newxyz, newpyr, weight);
        }

    }
}
