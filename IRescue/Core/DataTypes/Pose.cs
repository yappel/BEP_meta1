﻿// <copyright file="Pose.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
    using IRescue.Core.Datatypes;

    /// <summary>
    /// A pose of an object or person. Contains the orientation and position.
    /// </summary>
    public class Pose
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pose"/> class.
        /// </summary>
        /// <param name="position">The position of the object/person in cartesian coordinates</param>
        /// <param name="orientation">The orientation of the object/person in degrees</param>
        public Pose(Vector3 position, Vector3 orientation)
        {
            this.Position = position;
            this.Orientation = orientation;
        }

        /// <summary>
        /// Gets or sets the position of the pose.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the position of the pose.
        /// </summary>
        public Vector3 Orientation { get; set; }
    }
}