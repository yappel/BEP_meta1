// <copyright file="MonteCarloParticle.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
using System;
using IRescue.Core.DataTypes;

namespace IRescue.UserLocalisation.Particle
{
    /// <summary>
    /// TODO
    /// </summary>
    public class MonteCarloParticle
    {
        public Pose pose { get; set; }
        public float Weight { get; set; }


        public MonteCarloParticle(Vector3 xyz, Vector3 pyr, float weight)
        {
            this.pose = new Pose(xyz, pyr);
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
