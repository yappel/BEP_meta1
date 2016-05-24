// <copyright file="MarkerSensorInOutTests.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors.Marker;
using Moq;
using NUnit.Framework.Internal;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// Tests if the output is correct given a certain input.
    /// </summary>
    public class MarkerSensorInOutTests
    {
        /// <summary>
        /// The markerlocations. (Mocking not possible due to limitations of moq.
        /// </summary>
        private MarkerLocations mloc;

        [Test]
        public void TestIt()
        {
            float[][] data = new float[][]
            {
                new float[] {1,0,1, 0,0,0, 2,0,1, 0,180,0, 3,0,2, 0,180,0},
                new float[] {2.98f,0,2.06f, 0,290.99f,0, 0.15f,0,2.06f, 0,230.52f,0, 1.11f,0,1.18f, 0,60.47f,0},
                new float[] {2.98f,0,2.06f, 0,1.15f,0,  1.99f,0,0.57f,  0,10.43f,0, 1.11f,0,1.18f,  0,350.72f,0},
                new float[] {-1.4f,0,-1.72f, 0,154.32f,0, 2.3f,0,2.01f, 0,121.44f,0, -4.42f,0,-2.15f, 0,32.88f,0},
                new float[] {-1.4f,-1.72f,0, 0,0,205.68f, 1.02f,5.29f,0, 0,0,354.05f, -3.3f,3.32f,0, 0,0,211.62f},
                new float[] {2,0,2 , 0,225,0 , 0,0,1.4142f , 0,180,0 , 1,0,1 , 0,45,0},
                new float[] {2,0,2 , 0,-90,0 , 1,0,1 , 0,180,0 , 1,0,3 , 0,90,0 },
                new float[] {2,0,2 , 0,90,0 , 1,0,1 , 0,180,0 , 3,0,1 , 0,-90,0 },
            };
            mloc = new MarkerLocations();
            for (int i = 0; i < data.Length; i++)
            {
                Pose marker = new Pose(
                    new Vector3(
                        data[i][0],
                        data[i][1],
                        data[i][2]),
                    new Vector3(
                        data[i][3],
                        data[i][4],
                        data[i][5]));
                Pose meting = new Pose(
                    new Vector3(
                        data[i][6],
                        data[i][7],
                        data[i][8]),
                    new Vector3(
                        data[i][9],
                        data[i][10],
                        data[i][11]));
                Pose output = new Pose(
                    new Vector3(
                        data[i][12],
                        data[i][13],
                        data[i][14]),
                    new Vector3(
                        data[i][15],
                        data[i][16],
                        data[i][17]));
                var i1 = i;
                System.Diagnostics.Debug.WriteLine(i);
                this.mloc.AddMarker(i1, marker);
                this.TestThis(meting, output, i);
            }

        }

        public void TestThis(Pose meting, Pose output, int i)
        {
            MarkerSensor sensor = new MarkerSensor(0, 0, this.mloc);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(i, meting);
            sensor.UpdateLocations(1, dic);
            this.AssertVectorAreEqual(output.Position, sensor.GetLastPosition().Data);
            this.AssertVectorAreEqual(output.Orientation, sensor.GetLastOrientation().Data);
        }

        /// <summary>
        /// Assert that all elements in the vectors match with possible deviation 0.0001.
        /// </summary>
        /// <param name="expected">The expected vector.</param>
        /// <param name="actual">The actual vector.</param>
        private void AssertVectorAreEqual(Vector3 expected, Vector3 actual)
        {
            System.Diagnostics.Debug.WriteLine("x=" + actual.X + " y=" + actual.Y + " z=" + actual.Z);
            Assert.AreEqual(expected.X, actual.X, 0.01);
            Assert.AreEqual(expected.Y, actual.Y, 0.01);
            Assert.AreEqual(expected.Z, actual.Z, 0.01);
        }
    }
}
