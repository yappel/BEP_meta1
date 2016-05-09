using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra.Single;
using IRescue.Core.DataTypes;

namespace Core.Test
{
    public class Vector3Test
    {
        [Test]
        public void TestConstructor()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            Assert.True(vector is DenseVector);
        }
    }
}
