// <copyright file="FieldSize.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.Core.DataTypes
{
    /// <summary>
    /// The size of the playfield.
    /// </summary>
    public struct FieldSize
    {
        /// <summary>
        /// The minimum coordinate on the X axis.
        /// </summary>
        public float Xmin;

        /// <summary>
        /// The maximum coordinate on the X axis.
        /// </summary>
        public float Xmax;

        /// <summary>
        /// The minimum coordinate on the Y axis.
        /// </summary>
        public float Ymin;

        /// <summary>
        /// The maximum coordinate on the Y axis.
        /// </summary>
        public float Ymax;

        /// <summary>
        /// The minimum coordinate on the Z axis.
        /// </summary>
        public float Zmin;

        /// <summary>
        /// The maximum coordinate on the Z axis.
        /// </summary>
        public float Zmax;
    }
}
