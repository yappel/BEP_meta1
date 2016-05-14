// <copyright file="PositionScenario.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;

namespace IRescue.UserLocalisationMeasuring.DataGeneration
{
    /// <summary>
    /// Simulates data
    /// </summary>
    public class PositionScenario : IPositionSource
    {
        public Measurement<Vector3> GetLastPosition()
        {
            throw new System.NotImplementedException();
        }

        public Measurement<Vector3> GetPosition(long timeStamp)
        {
            throw new System.NotImplementedException();
        }

        public List<Measurement<Vector3>> GetPositions(long startTimeStamp, long endTimeStamp)
        {
            throw new System.NotImplementedException();
        }

        public List<Measurement<Vector3>> GetAllPositions()
        {
            throw new System.NotImplementedException();
        }
    }
}
