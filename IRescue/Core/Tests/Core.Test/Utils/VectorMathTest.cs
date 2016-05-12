// <copyright file="VectorMathTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// Test class for testing the <see cref="IRescue.Core.Utils.VectorMath"/> class.
    /// </summary>
    public class VectorMathTest
    {
        /// <summary>
        /// Test that a simple 90 degrees rotation around the X axis returns the correct result.
        /// </summary>
        [Test]
        public void RotateVectorSimpleXAxisRotationTest()
        {
            Vector3 vec = new Vector3(0, 1, 0);
            IRescue.Core.Utils.VectorMath.RotateVector(vec, 90, 0, 0);
            Assert.AreEqual(0, vec.X, 0.01f);
            Assert.AreEqual(0, vec.Y, 0.01f);
            Assert.AreEqual(1, vec.Z, 0.01f);
        }

        /// <summary>
        /// Test that a simple 90 degrees rotation around the Y axis returns the correct result. 
        /// </summary>
        [Test]
        public void RotateVectorSimpleYAxisRotationTest()
        {
            Vector3 vec = new Vector3(0, 0, 1);
            IRescue.Core.Utils.VectorMath.RotateVector(vec, 0, 90, 0);
            Assert.AreEqual(1, vec.X, 0.01f);
            Assert.AreEqual(0, vec.Y, 0.01f);
            Assert.AreEqual(0, vec.Z, 0.01f);
        }

        /// <summary>
        /// Test that a simple 90 degrees rotation around the Z axis returns the correct result.
        /// </summary>
        [Test]
        public void RotateVectorSimpleZAxisRotationTest()
        {
            Vector3 vec = new Vector3(0, 1, 0);
            IRescue.Core.Utils.VectorMath.RotateVector(vec, 0, 0, 90);
            Assert.AreEqual(-1, vec.X, 0.01f);
            Assert.AreEqual(0, vec.Y, 0.01f);
            Assert.AreEqual(0, vec.Z, 0.01f);
        }
    }
}