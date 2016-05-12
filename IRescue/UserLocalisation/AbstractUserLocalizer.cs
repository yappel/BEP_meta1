// <copyright file="AbstractUserLocalizer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation
{
    using System;
    using System.Collections.Generic;
    using Core.DataTypes;

    /// <summary>
    ///   Abstract class for user localization. Every child will implement their own filter.
    /// </summary>
    public abstract class AbstractUserLocalizer
    {
        /// <summary>
        ///   Position coordinates.
        /// </summary>
        private Vector3 position;

        /// <summary>
        ///   Rotation coordinates.
        /// </summary>
        private Vector3 rotation;

        /// <summary>
        ///   Calculate the new location of the user based on the visible markers and accelerometer data.
        /// </summary>
        /// <param name="locations">Predicted locations based on data like visible markers and GPS.</param>
        public abstract void ProcessLocation(List<Object> locations);

        /// <summary>
        ///  Return a Vector3 with the calculated position.
        /// </summary>
        /// <returns>Vector3 the position</returns>
        public Vector3 GetPosition()
        {
            return this.position;
        }

        /// <summary>
        ///  Return a Vector 3 with the calculated rotation.
        /// </summary>
        /// <returns> Vector 3 </returns>
        public Vector3 GetRotation()
        {
            return this.rotation;
        }
    }
}