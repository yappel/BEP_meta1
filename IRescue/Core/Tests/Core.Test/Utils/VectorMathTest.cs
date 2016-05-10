// <copyright file="VectorMathTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    public class VectorMathTest
    {
        [Test]
        public void RotateVectorSimpleRollTest()
        {
            Vector3 vec = new Vector3(0, 1, 0);
            Vector3 res = IRescue.Core.Utils.VectorMath.RotateVector(vec, 90, 0, 0);
            Assert.AreEqual(0, res.X, 0.01f);
            Assert.AreEqual(0, res.Y, 0.01f);
            Assert.AreEqual(1, res.Z, 0.01f);
        }

        [Test]
        public void RotateVectorSimplePitchTest()
        {
            Vector3 vec = new Vector3(0, 0, 1);
            Vector3 res = IRescue.Core.Utils.VectorMath.RotateVector(vec, 0, 90, 0);
            Assert.AreEqual(1, res.X, 0.01f);
            Assert.AreEqual(0, res.Y, 0.01f);
            Assert.AreEqual(0, res.Z, 0.01f);
        }

        [Test]
        public void RotateVectorSimpleYawTest()
        {
            Vector3 vec = new Vector3(0, 1, 0);
            Vector3 res = IRescue.Core.Utils.VectorMath.RotateVector(vec, 0, 0, 90);
            Assert.AreEqual(-1, res.X, 0.01f);
            Assert.AreEqual(0, res.Y, 0.01f);
            Assert.AreEqual(0, res.Z, 0.01f);
        }
    }
}