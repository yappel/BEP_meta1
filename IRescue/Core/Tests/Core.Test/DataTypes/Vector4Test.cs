// <copyright file="Vector4Test.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using System;
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// Test class for the <see cref="Vector4"/> class.
    /// Testing setters, getters, constructor and other functionality.
    /// </summary>
    public class Vector4Test
    {
        /// <summary>
        /// Test that creating a Vector4 with no parameters sets all values to 0.
        /// </summary>
        [Test]
        public void EmptyConstructorTest()
        {
            Vector4 vector = new Vector4();
            Assert.AreEqual(0, vector.X);
            Assert.AreEqual(0, vector.Y);
            Assert.AreEqual(0, vector.Z);
            Assert.AreEqual(0, vector.W);
        }

        /// <summary>
        /// Test that the constructor sets all the values correctly.
        /// </summary>
        [Test]
        public void ConstructorTest()
        {
            Vector4 vector = new Vector4(1, 2, 3, 4);
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(3, vector.Z);
            Assert.AreEqual(4, vector.W);
        }

        /// <summary>
        /// Test that a constructor without w value sets all values correctly
        /// and sets the w value to 1.
        /// </summary>
        [Test]
        public void ConstructorNoWValueTest()
        {
            Vector4 vector = new Vector4(2, 3, 4);
            Assert.AreEqual(2, vector.X);
            Assert.AreEqual(3, vector.Y);
            Assert.AreEqual(4, vector.Z);
            Assert.AreEqual(1, vector.W);
        }

        /// <summary>
        /// Test that a constructor with an array of floats sets all the values correctly.
        /// </summary>
        [Test]
        public void ConstructorFromArrayTest()
        {
            Vector4 vector = new Vector4(new float[] { 1, 2, 3, 4 });
            Assert.AreEqual(1, vector.X);
            Assert.AreEqual(2, vector.Y);
            Assert.AreEqual(3, vector.Z);
            Assert.AreEqual(4, vector.W);
        }

        /// <summary>
        /// Test that an incorrect array size throws an ArgumentException.
        /// </summary>
        [Test]
        public void ConstructorFromArrayIncorrectSize()
        {
            Assert.Catch<ArgumentException>(() => new Vector4(new float[] { 1 }));
        }

        /// <summary>
        /// Test that a constructor with a Vector3 gives the correct values.
        /// </summary>
        [Test]
        public void ConstructorFromVector3Test()
        {
            Vector3 v3 = new Vector3(1, 2, 3);
            Vector4 v4 = new Vector4(v3, 4);
            Assert.AreEqual(v3.X, v4.X);
            Assert.AreEqual(v3.Y, v4.Y);
            Assert.AreEqual(v3.Z, v4.Z);
            Assert.AreEqual(4, v4.W);
        }

        /// <summary>
        /// Test that a constructor with a Vector3 and no w value gives the correct values and w default value.
        /// </summary>
        [Test]
        public void ConstructorFromVector3NoWValueTest()
        {
            Vector3 v3 = new Vector3(2, 3, 4);
            Vector4 v4 = new Vector4(v3);
            Assert.AreEqual(v3.X, v4.X);
            Assert.AreEqual(v3.Y, v4.Y);
            Assert.AreEqual(v3.Z, v4.Z);
            Assert.AreEqual(1, v4.W);
        }

        /// <summary>
        /// Test that the getters and setters function correctly.
        /// </summary>
        [Test]
        public void GettersAndSettersTest()
        {
            Vector4 v = new Vector4();
            v.X = 1;
            v.Y = 2;
            v.Z = 3;
            v.W = 4;
            Assert.AreEqual(1, v.X);
            Assert.AreEqual(2, v.Y);
            Assert.AreEqual(3, v.Z);
            Assert.AreEqual(4, v.W);
        }
    }
}
