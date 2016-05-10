// <copyright file="MonteCarloLocalizerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>


using IRescue.UserLocalisation.Sensors;
using Moq;

namespace IRescue.UserLocalisation.Particle
{
    using Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class MonteCarloLocalizerTest
    {


        [SetUp]
        public void Init()
        {
            //DynamicMock mockview = null;
        }


        /// <summary>
        /// Tests if the right amount of particles gets created.
        /// </summary>
        [Test]
        public void TestInitParticleAmount()
        {
            var expected = 100;
            var localizer = new MonteCarloLocalizer(expected, new Vector3(), new Vector3());
            Assert.AreEqual(expected, localizer.Particlelist.Count);

            expected += 1;
            var notexpected = 100;
            localizer = new MonteCarloLocalizer(expected, new Vector3(), new Vector3());
            Assert.AreNotEqual(notexpected, localizer.Particlelist.Count);
        }

        /// <summary>
        /// Test if the filter is able to give a predition.
        /// </summary>
        [Test]
        public void TestRun()
        {
            MonteCarloLocalizer loca = new MonteCarloLocalizer(200, new Vector3(500, 2, 500), new Vector3(360, 360, 360));
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            possourcemock.Setup(foo => foo.GetPosition(0)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 0), 100, 0));
            possourcemock.Setup(foo => foo.GetPosition(1)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 1), 100, 1));
            possourcemock.Setup(foo => foo.GetPosition(2)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 2), 100, 2));
            var orisourcemock = new Mock<IOrientationSource>();
            orisourcemock.Setup(foo => foo.GetOrientation(0)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 0), 100, 0));
            orisourcemock.Setup(foo => foo.GetOrientation(1)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 0), 100, 1));
            orisourcemock.Setup(foo => foo.GetOrientation(2)).Returns(new Measurement<Vector3>(new Vector3(0, 1.8f, 0), 100, 2));
            loca.AddPositionSource(possourcemock.Object);
            loca.AddOrientationSource(orisourcemock.Object);
            Assert.NotNull(loca.CalculatePose(0));
            Assert.NotNull(loca.CalculatePose(1));
            Assert.NotNull(loca.CalculatePose(2));
        }
    }
}
