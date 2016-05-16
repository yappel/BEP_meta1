// <copyright file="LinearPredicterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using Core.DataTypes;
    using NUnit.Framework;
    using PosePrediction;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class LinearPredicterTest
    {
        /// <summary>
        /// Test if the predicted translation when no previous info is know is 0 in every dimension.
        /// </summary>
        [Test]
        public void TestPredict()
        {
            LinearPosePredicter pred = new LinearPosePredicter();
            float[] output = pred.PredictPoseAt(1);
            Assert.AreEqual(new float[] { 0, 0, 0, 0, 0, 0 }, output);
        }

        /// <summary>
        /// Test if the predictions are linear.
        /// </summary>
        [Test]
        public void TestPredictWithPrevs()
        {
            LinearPosePredicter pred = new LinearPosePredicter();
            pred.AddPoseData(1, new Pose(new Vector3(1, 1, 1), new Vector3()));
            pred.AddPoseData(2, new Pose(new Vector3(2, 2, 2), new Vector3()));
            float[] output = pred.PredictPoseAt(3);
            float[] expected = new float[] { 1, 1, 1, 0, 0, 0 };
            Assert.AreEqual(expected, output);
            expected = new float[] { 3, 3, 3, 0, 0, 0 };
            Assert.AreEqual(expected, pred.PredictPoseAt(5));
        }

        /// <summary>
        /// Test putting in a wrong timestamp
        /// </summary>
        [Test]
        public void TestInvalidTimeStamp()
        {
            LinearPosePredicter pred = new LinearPosePredicter();
            Assert.Throws<ArgumentException>(() => pred.AddPoseData(-2, null));
        }

        /// <summary>
        /// Test putting in a wrong timestamp
        /// </summary>
        [Test]
        public void TestInvalidTimeStamp2()
        {
            LinearPosePredicter pred = new LinearPosePredicter();
            pred.AddPoseData(1, new Pose(new Vector3(1, 1, 1), new Vector3()));
            pred.AddPoseData(2, new Pose(new Vector3(2, 2, 2), new Vector3()));
            Assert.Throws<ArgumentException>(() => pred.PredictPoseAt(0));
        }
    }
}
