// <copyright file="LinearPredicterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
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
            LinearPredicter pred = new LinearPredicter();
            float[] output = pred.Predict(new Vector3(), new Vector3(), 1, 1);
            Assert.AreEqual(new float[] { 0, 0, 0 }, output);
        }

        /// <summary>
        /// Test if the predictions are linear.
        /// </summary>
        [Test]
        public void TestPredictWithPrevs()
        {
            LinearPredicter pred = new LinearPredicter();
            pred.AddPose(new Pose(new Vector3(1, 1, 1), new Vector3()), 1);
            pred.AddPose(new Pose(new Vector3(2, 2, 2), new Vector3()), 2);
            float[] output = pred.PredictPositionAt(3);
            float[] expected = new float[] { 1, 1, 1, 0, 0, 0 };
            Assert.AreEqual(expected, output);
            expected = new float[] { 3, 3, 3, 0, 0, 0 };
            Assert.AreEqual(expected, pred.PredictPositionAt(5));
        }
    }
}
