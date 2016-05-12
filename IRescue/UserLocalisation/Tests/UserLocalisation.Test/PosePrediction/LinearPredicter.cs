﻿// <copyright file="IRNuniTest1.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Linq;
using IRescue.Core.DataTypes;
using UserLocalisation.PositionPrediction;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class IRNuniTest1
    {
        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestPredict()
        {
            LinearPredicter pred = new LinearPredicter();
            float[] output = pred.predict(new Vector3(), new Vector3(), 1, 1);
            Assert.AreEqual(0, output.Sum());
        }
    }
}
