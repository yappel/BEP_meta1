// <copyright file="VectorMathTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.Utils
{
    using System;
    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using NUnit.Framework;

    /// <summary>
    /// Test class for testing the <see cref="IRescue.Core.Utils.VectorMath"/> class.
    /// </summary>
    [TestFixture]
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

        /// <summary>
        /// Test changing the legnth of a vector.
        /// </summary>
        [Test]
        public void SetLengthVectorTest()
        {
            Vector<float> vec = new DenseVector(new[] { 0f, 1f });
            float desiredLength = 1000;
            VectorMath.SetLength(vec, desiredLength);
            Assert.AreEqual(desiredLength, vec.L2Norm());
        }

        /// <summary>
        /// Test changing the length of a vector to 0.
        /// </summary>
        [Test]
        public void SetLengthVectorToZeroTest()
        {
            Vector<float> vec = new DenseVector(new[] { 0f, 1f });
            VectorMath.SetLength(vec, 0);
            Assert.AreEqual(new[] { 0f, 0f }, vec.ToArray());
        }

        /// <summary>
        /// Test for the argument exception from the Vector2ToAngle method
        /// </summary>
        [Test]
        public void Vector2ToAngleException()
        {
            Vector<float> vector = new DenseVector(new[] { 0f, 1f, 2f });
            Assert.That(() => VectorMath.Vector2ToAngle(vector), Throws.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// Test for the RotateVector method
        /// </summary>
        [Test]
        public void RotateTest()
        {
            Vector3 vector = new Vector3(0, 0, 90);
            RotationMatrix rm = new RotationMatrix(90, 0, 0);
            VectorMath.RotateVector(vector, rm);
            Assert.AreEqual(0, vector.X, float.Epsilon);
            Assert.AreEqual(-90, vector.Y, float.Epsilon);
            Assert.AreEqual(0, vector.Z, 0.0001f);
        }

        /// <summary>
        /// Test normalizing a few orientation vectors.
        /// </summary>
        [Test]
        public void TestNormalize3dVector()
        {
            Vector3 vector1 = new Vector3(180, 180, 180);
            Vector3 vector2 = new Vector3(0, 0, 0);
            vector1 = VectorMath.Normalize(vector1);
            vector2 = VectorMath.Normalize(vector2);
            this.AreEqual(vector2, vector1);
            vector1 = new Vector3(180, 0, 180);
            vector2 = new Vector3(0, 180, 0);
            vector1 = VectorMath.Normalize(vector1);
            vector2 = VectorMath.Normalize(vector2);
            this.AreEqual(vector2, vector1);
            vector1 = new Vector3(2, 18, -78);
            vector2 = new Vector3(2, 18, -78);
            vector1 = VectorMath.Normalize(vector1);
            vector2 = VectorMath.Normalize(vector2);
            this.AreEqual(vector2, vector1);
        }

        /// <summary>
        /// Test converting vector to angle.
        /// </summary>
        [Test]
        public void TestVectorToAngle()
        {
            Vector v = new DenseVector(new float[] { -0.9987534f, -0.03564889f });
            Assert.AreEqual(180, VectorMath.Vector2ToAngle(v));
        }

        private void AreEqual(Vector3 v1, Vector3 v2)
        {
            Assert.AreEqual(v1.X, v2.X, 10E-05);
            Assert.AreEqual(v1.Y, v2.Y, 10E-05);
            Assert.AreEqual(v1.Z, v2.Z, 10E-05);
        }
    }
}