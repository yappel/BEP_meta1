namespace Core.Test.DataTypes
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    class TransformationMatrixTest
    {
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
            System.Diagnostics.Debug.WriteLine("x=" + res.X + " y=" + res.Y + " z=" + res.Z + " w=" + res.W);
            Assert.AreEqual(3.000000000000000, res.X);
            Assert.AreEqual(1.483563916494110, res.Y);
            Assert.AreEqual(1.673032607475615, res.Z);
            Assert.AreEqual(1, res.W);
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
    }
}
