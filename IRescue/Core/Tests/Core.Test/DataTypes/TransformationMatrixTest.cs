// <copyright file="TransformationMatrixTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// Test class for testing the <see cref="TransformationMatrix"/> class.
    /// Also tests some simple rotations, translations and rotation/translation combinations.
    /// </summary>
    public class TransformationMatrixTest
    {
        /// <summary>
        /// Reference to the transformation matrix to use in testing.
        /// </summary>
        private TransformationMatrix transformation;

        /// <summary>
        /// Test that a simple translation results in the correct output vector.
        /// </summary>
        [Test]
        public void SimpleTransformationTest()
        {
            this.transformation = new TransformationMatrix(1, 2, 1, 0, 0, 0);
            Vector4 res = new Vector4(2, 3, 2, 1);
            this.transformation.Multiply(res, res);
            Assert.AreEqual(3, res.X);
            Assert.AreEqual(5, res.Y);
            Assert.AreEqual(3, res.Z);
            Assert.AreEqual(1, res.W);
        }

        /// <summary>
        /// Test that a simple rotation results in the correct output vector.
        /// </summary>
        [Test]
        public void SimpleRotationTest()
        {
            this.transformation = new TransformationMatrix(0, 0, 0, 45, 90, 30);
            Vector4 res = new Vector4(1, 2, 3, 1);
            this.transformation.Multiply(res, res);
            Assert.AreEqual(3.4154, res.X, 0.0001);
            Assert.AreEqual(1.1554, res.Y, 0.0001);
            Assert.AreEqual(-1.0000, res.Z, 0.0001);
            Assert.AreEqual(1, res.W, 0.0001);
        }

        /// <summary>
        /// Test that a simple rotation and translation combination results in the correct vector.
        /// </summary>
        [Test]
        public void SimpleRotationTranslationTest()
        {
            this.transformation = new TransformationMatrix(1, 2, 3, 0, 90, 0);
            Vector4 res = new Vector4(1, 1, 1, 1);
            this.transformation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector4(2, 3, 2, 1), res);
        }

        /// <summary>
        /// Test that a transformation matrix with no arguments creates a diagonal matrix.
        /// Multiplying with a vector returns that same vector.
        /// </summary>
        [Test]
        public void NoTranslationNoRotationTest()
        {
            this.transformation = new TransformationMatrix();
            Vector4 res = new Vector4(1, 2, 3, 4);
            this.transformation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector4(1, 2, 3, 4), res);
        }

        /// <summary>
        /// Test that a transformation matrix created from 2 Vector3 vectors returns the correct result.
        /// </summary>
        [Test]
        public void TransformationMatrixFromVector3Test()
        {
            this.transformation = new TransformationMatrix(new Vector3(3, 2, 1), new Vector3(0, 90, 0));
            Vector4 res = new Vector4(1, 2, 3, 4);
            this.transformation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector4(15, 10, 3, 4), res);
        }

        /// <summary>
        /// Test that a transformation matrix created from 2 Vector3 vectors and w value returns the correct result.
        /// </summary>
        [Test]
        public void TransformationMatrixFromVector3AndWTest()
        {
            this.transformation = new TransformationMatrix(new Vector3(3, 2, 1), new Vector3(0, 90, 0), 2);
            Vector4 res = new Vector4(1, 2, 3, 4);
            this.transformation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector4(15, 10, 3, 8), res);
        }

        /// <summary>
        /// Test that a simple translation and rotationmatrix return the correct transformation matrix.
        /// </summary>
        [Test]
        public void TransformationMatrixFromRotationMatrixTest()
        {
            this.transformation = new TransformationMatrix(1, 2, 3, new RotationMatrix(0, 90, 0));
            Vector4 res = new Vector4(1, 1, 1, 1);
            this.transformation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector4(2, 3, 2, 1), res);
        }

        /// <summary>
        /// Test that extracting the rotation matrix returns a correct rotation matrix.
        /// </summary>
        [Test]
        public void GetRotationMatrixTest()
        {
            this.transformation = new TransformationMatrix(1, 1, 1, 45, 90, 30);
            RotationMatrix rot = this.transformation.GetRotation();
            RotationMatrix expected = new RotationMatrix(45, 90, 30);
            Assert.AreEqual(expected, rot);
        }

        /// <summary>
        /// Test that the orientation field returns the correct orientation.
        /// </summary>
        [Test]
        public void TransformationMatrixOrientationTest()
        {
            this.transformation = new TransformationMatrix(1, 1, 1, 45, 90, 180);
            Vector3 res = this.transformation.Orientation;
            Assert.AreEqual(45, res.X, 0.0001);
            Assert.AreEqual(90, res.Y, 0.0001);
            Assert.AreEqual(180, res.Z, 0.0001);
        }

        /// <summary>
        /// Test that the position field returns the correct position.
        /// </summary>
        [Test]
        public void TransformationMatrixPositionTest()
        {
            this.transformation = new TransformationMatrix(1, 2, 3, 0, 0, 0);
            Vector3 res = this.transformation.Position;
            Assert.AreEqual(1, res.X, 0.0001);
            Assert.AreEqual(2, res.Y, 0.0001);
            Assert.AreEqual(3, res.Z, 0.0001);
        }

        /// <summary>
        /// Assert that two vectors are equal.
        /// </summary>
        /// <param name="expected">The expected result.</param>
        /// <param name="actual">The actual result.</param>
        private void AssertVectorAreEqual(Vector4 expected, Vector4 actual)
        {
            Assert.AreEqual(expected.X, actual.X, 0.0001);
            Assert.AreEqual(expected.Y, actual.Y, 0.0001);
            Assert.AreEqual(expected.Z, actual.Z, 0.0001);
            Assert.AreEqual(expected.W, actual.W, 0.0001);
        }
    }
}
