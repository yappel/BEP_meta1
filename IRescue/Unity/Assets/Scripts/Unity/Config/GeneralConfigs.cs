using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Unity.Config
{
    using IniParser.Exceptions;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Sensors.IMU;
    using IRescue.UserLocalisation.Sensors.Marker;

    using UnityEngine;

    public class GeneralConfigs : AbstractConfigs
    {

        /// <summary>
        /// TODO
        /// </summary>
        private const string DefaultPath = "";

        private static readonly string DefaultUserPath = Application.persistentDataPath + "GeneralConfig.txt";

        private const string UserCanMoveKey = "usercanmove";

        private const string IgnoreIMUDataKey = "ignoreimudata";

        private const string IgnoreMarkersKey = "ignoremarkers";

        private const string TrackWaterKey = "trackwater";

        private const string JawsKey = "jaws";

        private const string LocalizerNameKey = "localizername";

        private IDistribution markerPosDist;

        private IDistribution markerOriDist;

        private IDistribution imuPosDist;

        private IDistribution imoOriDist;

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        /// <param name="path">Path to the configuration file to use.</param>
        public GeneralConfigs(string path, MarkerConfigs markerConfigs)
            : base(path, DefaultPath)
        {
            this.UserCanMove = this.GetBool(UserCanMoveKey);
            this.IgnoreIMUData = this.GetBool(IgnoreIMUDataKey);
            this.IgnoreMarkers = this.GetBool(IgnoreMarkersKey);
            this.TrackWater = this.GetBool(TrackWaterKey);
            this.Jaws = this.GetBool(JawsKey);
            this.UserLocalizer = this.CreateUserLocalizer();
            this.IMUSource = this.CreateIMUSource();
            this.MarkerSensor = this.CreateMarkerSensor(markerConfigs);

        }

        private MarkerSensor CreateMarkerSensor(MarkerConfigs markerConfigs)
        {
            if (this.IgnoreMarkers)
            {
                return null;
            }
            //MarkerSensor sensor = new MarkerSensor(markerConfigs, this.markerPosDist, this.markerOriDist);
            throw new NotImplementedException();
        }

        private IMUSource CreateIMUSource()
        {
            throw new NotImplementedException();
        }

        private AbstractUserLocalizer CreateUserLocalizer()
        {
            String localizerName;
            this.TryGetString(string.Empty, LocalizerNameKey, false, out localizerName);
            switch (localizerName)
            {
                case "particlefilter":
                    return this.CreateParticleFilter();
                default:
                    throw new ParsingException("Localizer name not recognized");
            }
        }

        private AbstractUserLocalizer CreateParticleFilter()
        {
            throw new NotImplementedException();
        }

        private bool GetBool(string key)
        {
            bool res;
            this.TryGetBool(string.Empty, key, false, out res);
            return res;
        }

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        public GeneralConfigs(MarkerConfigs markerConfigs) : this(DefaultUserPath, markerConfigs)
        {

        }

        /// <summary>
        /// The class used to determine the pose of the user at a certain times stamp.
        /// </summary>
        public AbstractUserLocalizer UserLocalizer { get; }

        public IMUSource IMUSource { get; }

        public MarkerSensor MarkerSensor { get; }

        /// <summary>
        /// If the user should be able to move around in the world.
        /// </summary>
        public bool UserCanMove { get; }

        /// <summary>
        /// If the IMU sensor should be ignored.
        /// </summary>
        public bool IgnoreIMUData { get; }

        /// <summary>
        /// If the marker tracking sensors should be ignored.
        /// </summary>
        public bool IgnoreMarkers { get; }

        /// <summary>
        /// If application should track the current water level.
        /// </summary>
        public bool TrackWater { get; }

        /// <summary>
        /// If jaws feature should be enabled.
        /// </summary>
        public bool Jaws { get; }

        /// <summary>
        /// The size of the trainingsarea.
        /// </summary>
        public FieldSize fieldSize { get; }
    }
}
