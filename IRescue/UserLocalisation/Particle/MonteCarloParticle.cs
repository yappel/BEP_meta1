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

        public void Translatexyz(Vector3 translation)
        {
            pose.Position.Add(translation);
        }

        public MonteCarloParticle duplicate()
        {
            var newxyz = new Vector3(this.pose.Position.Values);
            var newpyr = new Vector3(this.pose.Orientation.Values);
            return new MonteCarloParticle(newxyz, newpyr, this.Weight);
        }

    }
}
