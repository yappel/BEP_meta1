// <copyright file="MarkerConfigs.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.Config
{
    using System.Collections.Generic;

    using UnityEngine;

    using Vector3 = IRescue.Core.DataTypes.Vector3;

    /// <summary>
    /// Reads an INI config file about the placed markers in the world.
    ///  Uses default values when entries in this file are not valid/available.
    /// </summary>
    public class MarkerConfigs : AbstractMarkerConfigs<MarkerConfig>
    {
        /// <summary>
        /// TODO
        /// </summary>
        private const string DefaultPath = "";

        private const string OriXKey = "orix";

        private const string OriYKey = "oriy";

        private const string OriZKey = "oriz";

        private const string PosXKey = "posx";

        private const string PosYKey = "posy";

        private const string PosZKey = "posz";

        private const string SizeKey = "size";

        private static readonly string DefaultUserPath = Application.dataPath + "MarkerConfig.ini";

        private MarkerConfig defaultValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerConfigs"/> class.
        /// The configuration information about the markers places in the world.
        /// </summary>
        /// <param name="path">Path to the config file to use.</param>
        public MarkerConfigs(string path)
            : base(path, DefaultPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerConfigs"/> class.
        ///  The configuration information about the markers places in the world.
        /// </summary>
        public MarkerConfigs()
            : this(DefaultUserPath)
        {
        }

        protected override MarkerConfig CreateMarkerConfig(string section)
        {
            MarkerConfig newconfig = default(MarkerConfig);
            this.TryGetDouble(section, SizeKey, false, out newconfig.Size);
            this.TryGetVector3(section, PosXKey, PosYKey, PosZKey, false, out newconfig.Postion);
            this.TryGetVector3(section, OriXKey, OriYKey, OriZKey, false, out newconfig.Orientation);
            return newconfig;
        }
    }

    public struct MarkerConfig
    {
        /// <summary>
        /// The cartesian coordinates in meters.
        /// </summary>
        public Vector3 Postion;

        /// <summary>
        /// The Tait–Bryan angles in degrees.
        /// </summary>
        public Vector3 Orientation;

        /// <summary>
        /// The height and with of the marker in meters.
        /// </summary>
        public double Size;
    }
}