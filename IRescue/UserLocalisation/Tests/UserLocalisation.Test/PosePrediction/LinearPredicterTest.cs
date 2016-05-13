// <copyright file="RandomGeneratorTest.cs" company="Delft University of Technology">
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

        [Test]
        public void TestPredictWithPrevs()
        {
            LinearPredicter pred = new LinearPredicter();
            pred.addPose(new Pose(new Vector3(1, 1, 1), new Vector3()), 1);
            pred.addPose(new Pose(new Vector3(2, 2, 2), new Vector3()), 2);
            float[] output = pred.predictPositionAt(3);
            float[] expected = new float[] { 1, 1, 1, 0, 0, 0 };
            Assert.AreEqual(expected, output);
            expected = new float[] { 3, 3, 3, 0, 0, 0 };
            Assert.AreEqual(expected, pred.predictPositionAt(5));
        }
    }
}
