// <copyright file="IRotationSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Inputsensors
{
    using System.Collections.Generic;
  
    /// <summary>
    ///   A RotationSource provides information about the rotation of the user.
    /// </summary>
    public interface IRotationSource
    {
        /// <summary>
        /// Gets the rotation vectors.
        /// </summary>
        /// <returns>A SensorVector3 containing the rotation in the xyz dimensions and a deviation to specify the certainty of this data</returns>
        List<SensorVector3> GetRotations();
    }
}
