// <copyright file="AbstractScenario3D.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
            Func<float> noisex,
            Func<float> noisey,
            Func<float> noisez)
        {
            this.Dataset = new SortedDictionary<long, Measurement<Vector3>>();
            this.RealX = realx;
            this.RealY = realy;
            this.RealZ = realz;
            foreach (long timestamp in timestamps)
            {
                float x = realx(timestamp) + noisex();
                float y = realy(timestamp) + noisey();
                float z = realz(timestamp) + noisez();
                Vector3 data = new Vector3(x, y, z);
                float std = 0.1f;
                long timeStamp = timestamp;

                Measurement<Vector3> meas = new Measurement<Vector3>(data, std, timeStamp);
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

        /// <summary>
        /// Function describing the real X value at a time certain timestamp
        /// </summary>
        public Func<long, float> NoiseX { get; }

        /// <summary>
        /// Function describing the real Y value at a time certain timestamp
        /// </summary>
        public Func<long, float> NoiseY { get; }

        /// <summary>
        /// Function describing the real Z value at a time certain timestamp
        /// </summary>
        public Func<long, float> NoiseZ { get; }
    }
}
