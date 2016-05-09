// <copyright file="ILocationSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.Core.DataTypes;

namespace IRescue.UserLocalisation.Sensors
{
    using System.Collections.Generic;
  
    /// <summary>
    ///   A LocationSource provides information about the location of the user.
    /// </summary>
    public interface ILocationSource
    {
        /// <summary>
        /// Gets the location vectors.
        /// </summary>
        /// <returns>A SensorVector3 containing the location in the xyz dimensions in meters and a deviation to specify the certainty of this data</returns>
        List<SensorVector3> GetLocations();
    }
}
