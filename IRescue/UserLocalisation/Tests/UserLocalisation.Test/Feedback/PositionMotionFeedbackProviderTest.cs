// <copyright file="PositionMotionFeedbackProviderTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Feedback;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class PositionMotionFeedbackProviderTest
    {
        /// <summary>
        /// Test subject.
        /// </summary>
        private PositionMotionFeedbackProvider provider;

        /// <summary>
        /// Mock for the feedback receiver.
        /// </summary>
        private Mock<IVelocityFeedbackReceiver> receivermock;

        /// <summary>
        /// Postion data for timestamp 1.
        /// </summary>
        private FeedbackData<Vector3> posdata1;

        /// <summary>
        /// Postion data for timestamp 1001.
        /// </summary>
        private FeedbackData<Vector3> posdata2;

        /// <summary>
        /// Expected velocity data at timestamp 1001.
        /// </summary>
        private FeedbackData<Vector3> expectedvel;

        /// <summary>
        /// Setup method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.provider = new PositionMotionFeedbackProvider();
            this.receivermock = new Mock<IVelocityFeedbackReceiver>();
            this.posdata1 = new FeedbackData<Vector3>()
            {
                Data = new Vector3(1, 1, 1),
                Stddev = 0.1f,
                TimeStamp = 1
            };
            this.posdata2 = new FeedbackData<Vector3>()
            {
                Data = new Vector3(2, 3, 4),
                Stddev = 0.2f,
                TimeStamp = 1001
            };
            this.expectedvel = new FeedbackData<Vector3>()
            {
                Data = new Vector3(1, 2, 3),
                Stddev = (float)Math.Sqrt(0.01 + 0.04),
                TimeStamp = 1001
            };
        }

        /// <summary>
        /// Test receiving velocity feedback.
        /// </summary>
        [Test]
        public void TestVelocityFeedback()
        {
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.NotifyPositionFeedback(this.posdata1);
            this.provider.NotifyPositionFeedback(this.posdata2);
            this.receivermock.Verify(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Once);
        }

        /// <summary>
        /// Test receiving correct velocity feedback.
        /// </summary>
        [Test]
        public void TestCorrectVelocityFeedback()
        {
            FeedbackData<Vector3> actual = default(FeedbackData<Vector3>);
            this.receivermock.Setup(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>())).Callback<FeedbackData<Vector3>>(d => actual = d);
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.NotifyPositionFeedback(this.posdata1);
            this.provider.NotifyPositionFeedback(this.posdata2);
            this.receivermock.Verify(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Once);
            Assert.AreEqual(this.expectedvel.Data, actual.Data);
            Assert.AreEqual(this.expectedvel.TimeStamp, actual.TimeStamp);
            Assert.AreEqual(this.expectedvel.Stddev, actual.Stddev);
        }

        /// <summary>
        /// Test not receiving velocity feedback.
        /// </summary>
        [Test]
        public void TestNoVelocityFeedback()
        {
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.UnregisterReceiver(this.receivermock.Object);
            this.provider.UnregisterReceiver(this.receivermock.Object);
            this.provider.NotifyPositionFeedback(this.posdata1);
            this.provider.NotifyPositionFeedback(this.posdata2);
            this.receivermock.Verify(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Never);
        }

        /// <summary>
        /// Test adding same receiver multiple times.
        /// </summary>
        [Test]
        public void TestMultipleVelReceivers()
        {
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.NotifyPositionFeedback(this.posdata1);
            this.provider.NotifyPositionFeedback(this.posdata2);
            this.receivermock.Verify(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Once);
        }

        /// <summary>
        /// Test that only data with timestamps later than the previous timestamp are getting processed.
        /// </summary>
        [Test]
        public void TestAddingInvalidData()
        {
            this.provider.RegisterReceiver(this.receivermock.Object);
            this.provider.NotifyPositionFeedback(this.posdata2);
            this.provider.NotifyPositionFeedback(this.posdata1);
            this.receivermock.Verify(f => f.NotifyVelocityFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Never);
        }
    }
}
