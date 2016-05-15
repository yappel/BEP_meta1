// <copyright file="AbstractScenario3D.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using IRescue.Core.DataTypes;

namespace IRescue.UserLocalisationMeasuring.DataGeneration
{
    /// <summary>
    /// TODO
    /// </summary>
    public abstract class AbstractScenario3D
    {
        protected AbstractScenario3D(
            Func<long, float> realx,
            Func<long, float> realy,
            Func<long, float> realz,
            long[] timestamps,
            Func<float> noise,
            float stddev)
        {
            this.Dataset = new SortedDictionary<long, Measurement<Vector3>>();
            this.RealX = realx;
            this.RealY = realy;
            this.RealZ = realz;
            foreach (long timestamp in timestamps)
            {
                float x = realx(timestamp) + noise();
                float y = realy(timestamp) + noise();
                float z = realz(timestamp) + noise();
                Vector3 data = new Vector3(x, y, z);
                long timeStamp = timestamp;
                Measurement<Vector3> meas = new Measurement<Vector3>(data, stddev, timeStamp);
                this.Dataset.Add(timestamp, meas);
            }
        }

        /// <summary>
        /// Dataset containing all the <see cref="Measurement{T}"/>
        /// </summary>
        public SortedDictionary<long, Measurement<Vector3>> Dataset { get; }

        /// <summary>
        /// Function describing the real X value at a time certain timestamp
        /// </summary>
        public Func<long, float> RealX { get; }

        /// <summary>
        /// Function describing the real Y value at a time certain timestamp
        /// </summary>
        public Func<long, float> RealY { get; }

        /// <summary>
        /// Function describing the real Z value at a time certain timestamp
        /// </summary>
        public Func<long, float> RealZ { get; }
    }
}
