// <copyright file="OrientationScenario.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;

namespace IRescue.UserLocalisationMeasuring.DataGeneration
{
    /// <summary>
    /// Provides data about the orientation over time based on a certain scenario. 
    /// It takes as input info about <see cref="Measurement{T}"/>s and 
    /// linearly generates its own measurements between these inputs depending on the given frequency that is desired.
    /// </summary>
    public class OrientationScenario : IOrientationSource
    {
        /// <summary>
        /// Dataset containing all the <see cref="Measurement{T}"/>
        /// </summary>
        private SortedDictionary<int, Measurement<Vector3>> dataset;



        /// <summary>
        /// Initializes a new instance of the <see cref="OrientationScenario"/> class.
        /// </summary>
        /// <param name="xmlfile">A <see cref="XDocument"/> containing information to derive <see cref="Measurement{T}"/>s from.</param>
        /// <param name="frequency">The amount of <see cref="Measurement{T}"/>s should be able to provide per second</param>
        public OrientationScenario(XDocument xmlfile, int frequency)
        {
            this.dataset = new SortedDictionary<int, Measurement<Vector3>>();
            var rootNodes = xmlfile.Root.DescendantNodes().OfType<XElement>();
            foreach (var node in (from xml in xmlfile.Descendants("Measurement") select xml))
            {
                //TODO convert data from xml to measurements and add to list
                float x = 0;
                float y = 0;
                float z = 0;
                Vector3 data = new Vector3(x, y, z);
                float std = 0;
                int timeStamp = 0;

                Measurement<Vector3> meas = new Measurement<Vector3>(data, std, timeStamp);
                this.dataset.Add(timeStamp, meas);
            }
            //TODO interpolate measurements so the list this.dataset contains "frequency" amount of measurements per second
        }

        public Measurement<Vector3> GetLastOrientation()
        {
            throw new System.NotImplementedException();
        }

        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            throw new System.NotImplementedException();
        }

        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public List<Measurement<Vector3>> GetAllOrientations()
        {
            throw new System.NotImplementedException();
        }
    }
}
