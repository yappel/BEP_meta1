// <copyright file="OrientationScenario.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;

namespace IRescue.UserLocalisationMeasuring.DataGeneration
{
    /// <summary>
    /// Provides data about the orientation over time based on a certain scenario. 
    /// It takes as input info about <see cref="Measurement{T}"/>s and 
    /// linearly generates its own measurements between these inputs depending on the given frequency that is desired.
    /// </summary>
    public class OrientationScenario : AbstractScenario3D, IOrientationSource
    {

        public OrientationScenario(
            Func<long, float> realx,
            Func<long, float> realy,
            Func<long, float> realz,
            long[] timestamps,
            Func<float> noise,
            float stddev) : base(realx, realy, realz, timestamps, noise, stddev)
        {
        }

        public Measurement<Vector3> GetLastOrientation()
        {
            return this.Dataset[this.Dataset.Count];
        }

        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            Measurement<Vector3> meas;
            if (this.Dataset.TryGetValue(timeStamp, out meas))
            {
                return meas;
            }
            else
            {
                return null;
            }
        }

        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> measurments = new List<Measurement<Vector3>>();
            for (long i = startTimeStamp; i <= endTimeStamp; i++)
            {
                measurments.Add(this.GetOrientation(i));
            }
            return measurments;
        }

        public List<Measurement<Vector3>> GetAllOrientations()
        {
            List<Measurement<Vector3>> measurments = new List<Measurement<Vector3>>();
            foreach (KeyValuePair<long, Measurement<Vector3>> keyValuePair in this.Dataset)
            {
                measurments.Add(keyValuePair.Value);
            }
            return measurments;
        }
    }
}
