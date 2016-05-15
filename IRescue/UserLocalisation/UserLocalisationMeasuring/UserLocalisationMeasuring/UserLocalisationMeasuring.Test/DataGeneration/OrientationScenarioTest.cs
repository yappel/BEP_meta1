// <copyright file="OrientationScenarioTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Xml.Linq;
using IRescue.UserLocalisationMeasuring.DataGeneration;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class OrientationScenarioTest
    {

        private XDocument posnochange;

        [OneTimeSetUp]
        public void setup()
        {
            XElement root = new XElement("Root", new XElement("Range",
                                new XElement("X", 5),
                                new XElement("Y", 2),
                                new XElement("Z", 5)
                                ));
            for (double i = 0; i < 30; i++)
            {
                XElement pos = new XElement("orientation");
                root.Add(pos);
                pos.Add(new XElement("TimeStamp", i));
                pos.Add(new XElement("X", 1));
                pos.Add(new XElement("Y", 1.8));
                pos.Add(new XElement("Z", i));
            }
            this.posnochange = new XDocument(root);
        }


        private double Polynomial(double x, params double[] scalars)
        {
            double y = 0;
            for (int i = 0; i < scalars.Length; i++)
            {
                y += Math.Pow(x, scalars.Length - 1) * scalars[i];
            }

            return y;
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod()
        {
            OrientationScenario scena = new OrientationScenario(this.posnochange);
        }
    }
}
