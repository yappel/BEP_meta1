// <copyright file="PersonalFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisation.Sensors.Marker;
using Moq;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class PersonalFilterTest
    {
        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void FullRunWithRawMarkerData()
        {
            const double epsilon = 1;
            Pose pose = new Pose();
            const int cycleamount = 20;
            for (int i = 0; i < cycleamount; i++)
            {
                Pose res = RunOnRawMarkerData();
                float ppx = pose.Position.X;
                float ppy = pose.Position.Y;
                float ppz = pose.Position.Z;
                float pox = pose.Orientation.X;
                float poy = pose.Orientation.Y;
                float poz = pose.Orientation.Z;
                float rpx = res.Position.X;
                float rpy = res.Position.Y;
                float rpz = res.Position.Z;
                float rox = res.Orientation.X;
                float roy = res.Orientation.Y;
                float roz = res.Orientation.Z;
                pose = new Pose(new Vector3(ppx + rpx, ppy + rpy, ppz + rpz),
                    new Vector3(pox + rox, poy + roy, poz + roz));
            }
            pose.Orientation.Divide((float)cycleamount, pose.Orientation);
            pose.Position.Divide((float)cycleamount, pose.Orientation);
            Assert.AreEqual(1, pose.Position.X, epsilon);
            Assert.AreEqual(1, pose.Position.Y, epsilon);
            Assert.AreEqual(1.8, pose.Position.Z, epsilon);
            //Assert.AreEqual(0, pose.Orientation.X, 5);
            //Assert.AreEqual(90, pose.Orientation.Y, 5);
            //Assert.AreEqual(0, pose.Orientation.Z, 5);

        }

        public Pose RunOnRawMarkerData()
        {

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(TestContext.CurrentContext.TestDirectory + "\\P1OR180.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
                MarkerSensor msens = new MarkerSensor(1, TestContext.CurrentContext.TestDirectory + "\\MarkerMapRealistic.xml");
                filter.AddOrientationSource(msens);
                filter.AddPositionSource(msens);
                Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
                Pose pose = null;
                string line;
                for (int i = 0; i < 10; i++)
                {
                    line = streamReader.ReadLine();
                    string[] strdata = line.Split(' ');
                    float[] posxyz = new float[]
                    {Convert.ToSingle(strdata[3]), Convert.ToSingle(strdata[4]), Convert.ToSingle(strdata[5])};
                    float[] orixyz = new float[]
                    {Convert.ToSingle(strdata[0]), Convert.ToSingle(strdata[1]), Convert.ToSingle(strdata[2])};
                    dic.Add(0,
                        new Pose(new Vector3(posxyz),
                            new Vector3(orixyz)));
                    msens.UpdateLocations(dic);
                    pose = filter.CalculatePose(i);
                    dic.Clear();
                    Assert.AreEqual(1, msens.GetLastPosition().Data.X, 0.5);
                    Assert.AreEqual(1, msens.GetLastPosition().Data.Y, 0.5);
                    Assert.AreEqual(1, msens.GetLastPosition().Data.Z, 0.5);
                    //Assert.AreEqual(0, msens.GetLastOrientation().Data.X, 8);
                    //Assert.AreEqual(90, msens.GetLastOrientation().Data.Y, 8);
                    //Assert.AreEqual(0, msens.GetLastOrientation().Data.Z, 8);
                }
                return pose;
            }
        }

        //[Test]
        public void sladkfj()
        {
            ParticleFilter filterr = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(4.0f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            var fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
            System.IO.File.WriteAllText(fullpath, "0 0 0" + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }
            Pose pose = filterr.CalculatePose(0);
            System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }
        }

        //[Test]
        public void writefile()
        {
            ParticleFilter filterr = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(4.0f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filterr.AddPositionSource(possourcemock.Object);

            var fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
            Pose pose = filterr.CalculatePose(1);
            System.IO.File.WriteAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }

            for (int j = 0; j < 60; j++)
            {
                fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
                pose = filterr.CalculatePose(j);
                System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
                for (int i = 0; i < 30; i++)
                {
                    var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                    System.IO.File.AppendAllText(fullpath,
                        filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
                }
            }
        }
    }

}
