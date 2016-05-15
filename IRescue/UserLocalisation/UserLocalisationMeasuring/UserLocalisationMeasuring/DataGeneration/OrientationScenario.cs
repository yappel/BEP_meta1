// <copyright file="OrientationScenario.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
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
    public class OrientationScenario : AbstractScenario3D, IOrientationSource
    {

        public OrientationScenario(
            Func<long, float> realx,
            Func<long, float> realy,
            Func<long, float> realz,
            long[] timestamps,
            Func<float> noisex,
            Func<float> noisey,
            Func<float> noisez) : base(realx, realy, realz, timestamps, noisex, noisey, noisez)
        {

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
