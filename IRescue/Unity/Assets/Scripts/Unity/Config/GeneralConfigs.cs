namespace Assets.Scripts.Unity.Config
{
    using System.Collections.Generic;

    using IniParser.Exceptions;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation;
    using IRescue.UserLocalisation.Sensors.IMU;
    using IRescue.UserLocalisation.Sensors.Marker;

    using UnityEngine;

    public class GeneralConfigs : AbstractConfigs
    {
        /// <summary>
        /// TODO
        /// </summary>
        private const string DefaultPath = "";

        private const string IgnoreIMUDataKey = "ignoreimudata";

        private const string IgnoreMarkersKey = "ignoremarkers";

        private const int IMUBuffersize = 60;

        private const string JawsKey = "jaws";

        private const string LocalizerNameKey = "localizername";

        private const string NoiseGeneratorKey = "noisegenerator";

        private const string ParticleAmountKey = "particleamount";

        private const string ParticleGeneratorKey = "particlegenerator";

        private const string ResampleNoiseSizeKey = "resamplernoise";

        private const string ResamplerKey = "resampler";

        private const string SmootherKey = "smoothingalgorithm";

        private const string TrackWaterKey = "trackwater";

        private const string UserCanMoveKey = "usercanmove";

        private const string FieldSizeXmaxKey = "xmax";
        private const string FieldSizeXminKey = "xmin";
        private const string FieldSizeYmaxKey = "ymax";
        private const string FieldSizeYminKey = "ymin";
        private const string FieldSizeZmaxKey = "zmax";
        private const string FieldSizeZminKey = "zmin";

        private static readonly string DefaultUserPath = Application.persistentDataPath + "GeneralConfig.txt";

        private Normal imoOriDist = new Normal(1);

        private Normal imuPosDist = new Normal(0.1);

        private IDistribution markerOriDist = new Normal(1);

        private IDistribution markerPosDist = new Normal(0.1);

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        /// <param name="path">Path to the configuration file to use.</param>
        public GeneralConfigs(string path, string defaultPath, MarkerConfigs markerConfigs)
            : base(path, defaultPath)
        {
            this.UserCanMove = this.GetBool(UserCanMoveKey);
            this.IgnoreIMUData = this.GetBool(IgnoreIMUDataKey);
            this.IgnoreMarkers = this.GetBool(IgnoreMarkersKey);
            this.TrackWater = this.GetBool(TrackWaterKey);
            this.Jaws = this.GetBool(JawsKey);
            this.fieldSize = this.CreateFieldSize();
            this.UserLocalizer = this.CreateUserLocalizer();
            this.IMUSource = this.CreateIMUSource();
            this.MarkerSensor = this.CreateMarkerSensor(markerConfigs);
        }

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        /// <param name="path">Path to the configuration file to use.</param>
        public GeneralConfigs(string path, MarkerConfigs markerConfigs)
            : this(path, DefaultPath, markerConfigs)
        {
            this.UserCanMove = this.GetBool(UserCanMoveKey);
            this.IgnoreIMUData = this.GetBool(IgnoreIMUDataKey);
            this.IgnoreMarkers = this.GetBool(IgnoreMarkersKey);
            this.TrackWater = this.GetBool(TrackWaterKey);
            this.Jaws = this.GetBool(JawsKey);
            this.fieldSize = this.CreateFieldSize();
            this.UserLocalizer = this.CreateUserLocalizer();
            this.IMUSource = this.CreateIMUSource();
            this.MarkerSensor = this.CreateMarkerSensor(markerConfigs);
        }

        private FieldSize CreateFieldSize()
        {
            FieldSize size = new FieldSize();
            this.TryGetFloat("fieldsize", FieldSizeXmaxKey, false, out size.Xmax);
            this.TryGetFloat("fieldsize", FieldSizeXminKey, false, out size.Xmin);
            this.TryGetFloat("fieldsize", FieldSizeYmaxKey, false, out size.Ymax);
            this.TryGetFloat("fieldsize", FieldSizeYminKey, false, out size.Ymin);
            this.TryGetFloat("fieldsize", FieldSizeZmaxKey, false, out size.Zmax);
            this.TryGetFloat("fieldsize", FieldSizeZminKey, false, out size.Zmin);
            return size;
        }

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        public GeneralConfigs(MarkerConfigs markerConfigs)
            : this(DefaultUserPath, markerConfigs)
        {
        }

        /// <summary>
        /// The size of the trainingsarea.
        /// </summary>
        public FieldSize fieldSize { get; private set; }

        /// <summary>
        /// If the IMU sensor should be ignored.
        /// </summary>
        public bool IgnoreIMUData { get; private set; }

        /// <summary>
        /// If the marker tracking sensors should be ignored.
        /// </summary>
        public bool IgnoreMarkers { get; private set; }

        public IMUSource IMUSource { get; private set; }

        /// <summary>
        /// If jaws feature should be enabled.
        /// </summary>
        public bool Jaws { get; private set; }

        public MarkerSensor MarkerSensor { get; private set; }

        /// <summary>
        /// If application should track the current water level.
        /// </summary>
        public bool TrackWater { get; private set; }

        /// <summary>
        /// If the user should be able to move around in the world.
        /// </summary>
        public bool UserCanMove { get; private set; }

        /// <summary>
        /// The class used to determine the pose of the user at a certain times stamp.
        /// </summary>
        public IUserLocalizer UserLocalizer { get; private set; }

        private IMUSource CreateIMUSource()
        {
            return this.IgnoreIMUData ? null : new IMUSource(this.imuPosDist, this.imoOriDist, IMUBuffersize);
        }

        private MarkerSensor CreateMarkerSensor(MarkerConfigs markerConfigs)
        {
            if (this.IgnoreMarkers)
            {
                return null;
            }

            Dictionary<int, Pose> markers = new Dictionary<int, Pose>();
            foreach (KeyValuePair<int, MarkerConfig> pair in markerConfigs.markerConfigs)
            {
                markers.Add(pair.Key, new Pose(pair.Value.Postion, pair.Value.Orientation));
            }

            MarkerSensor sensor = new MarkerSensor(new MarkerLocations(markers), this.markerPosDist, this.markerOriDist);
            return sensor;
        }

        private IUserLocalizer CreateParticleFilter()
        {
            int particleAmount;
            this.TryGetInt(string.Empty, ParticleAmountKey, false, out particleAmount);

            float resampleNoiseSize;
            this.TryGetFloat(string.Empty, ResampleNoiseSizeKey, false, out resampleNoiseSize);

            string particleGenerator;
            this.TryGetString(string.Empty, ParticleGeneratorKey, false, out particleGenerator);

            string resampler;
            this.TryGetString(string.Empty, ResamplerKey, false, out resampler);

            string noiseGenerator;
            this.TryGetString(string.Empty, NoiseGeneratorKey, false, out noiseGenerator);

            string smoother;
            this.TryGetString(string.Empty, SmootherKey, false, out smoother);

            return ParticleFilterFactory.Create(particleAmount, resampleNoiseSize, this.fieldSize, particleGenerator, resampler, noiseGenerator, smoother);
        }

        private IUserLocalizer CreateUserLocalizer()
        {
            string localizerName;
            this.TryGetString(string.Empty, LocalizerNameKey, false, out localizerName);
            switch (localizerName)
            {
                case "particlefilter":
                    return this.CreateParticleFilter();
                default:
                    throw new ParsingException("Localizer name not recognized");
            }
        }

        private bool GetBool(string key)
        {
            bool res;
            this.TryGetBool(string.Empty, key, false, out res);
            return res;
        }
    }
}