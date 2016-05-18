// <copyright file="PositionScenario.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;

namespace IRescue.UserLocalisationMeasuring.DataGeneration
{
    /// <summary>
    /// Simulates data
    /// </summary>
    public class PositionScenario : AbstractScenario3D, IPositionSource
    {

        public PositionScenario(
            Func<long, float> realx,
            Func<long, float> realy,
            Func<long, float> realz,
            long[] timestamps,
            Func<float> noise,
            float stddev) : base(realx, realy, realz, timestamps, noise, stddev)
        {
        }

        public Measurement<Vector3> GetLastPosition()
        {
            return this.Dataset[this.Dataset.Count];
        }

        public Measurement<Vector3> GetPosition(long timeStamp)
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

        public List<Measurement<Vector3>> GetPositions(long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> measurments = new List<Measurement<Vector3>>();
            for (long i = startTimeStamp; i <= endTimeStamp; i++)
            {
                measurments.Add(this.GetPosition(i));
            }
            return measurments;
        }

        public List<Measurement<Vector3>> GetAllPositions()
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
