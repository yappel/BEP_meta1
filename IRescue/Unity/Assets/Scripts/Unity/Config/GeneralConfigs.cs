namespace Assets.Scripts.Unity.Config
{
    using System;
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

        private const string IgnoreMarkersKey = "ignoremarkerdata";

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

        private static readonly string DefaultUserPath = Application.dataPath + "GeneralConfig.txt";



        private const string DefaultSection = "misc";

        private List<string> errors;

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        /// <param name="path">Path to the configuration file to use.</param>
        public GeneralConfigs(string path, string defaultPath, out List<string> errors)
            : base(path, defaultPath)
        {
            this.errors = new List<string>();
            this.UserCanMove = this.GetBool(UserCanMoveKey);
            this.IgnoreIMUData = this.GetBool(IgnoreIMUDataKey);
            this.IgnoreMarkers = this.GetBool(IgnoreMarkersKey);
            this.TrackWater = this.GetBool(TrackWaterKey);
            this.Jaws = this.GetBool(JawsKey);
            this.fieldSize = this.CreateFieldSize();
            this.UserLocalizer = this.CreateUserLocalizer();
            errors = this.errors;
        }

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        /// <param name="path">Path to the configuration file to use.</param>
        public GeneralConfigs(string path, out List<string> errors)
            : this(path, DefaultPath, out errors)
        {
        }

        private FieldSize CreateFieldSize()
        {
            FieldSize size = new FieldSize();
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeXmaxKey, false, out size.Xmax));
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeXminKey, false, out size.Xmin));
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeYmaxKey, false, out size.Ymax));
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeYminKey, false, out size.Ymin));
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeZmaxKey, false, out size.Zmax));
            this.errors.AddRange(this.TryGetFloat("fieldsize", FieldSizeZminKey, false, out size.Zmin));
            return size;
        }

        /// <summary>
        /// Instantiates a new <see cref="GeneralConfigs"/>.
        /// Contains information about the general configurations of the application.
        /// </summary>
        public GeneralConfigs(out List<string> errors)
            : this(DefaultUserPath, out errors)
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

        /// <summary>
        /// If jaws feature should be enabled.
        /// </summary>
        public bool Jaws { get; private set; }

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

        private IUserLocalizer CreateParticleFilter()
        {
            int particleAmount;
            this.errors.AddRange(this.TryGetInt(DefaultSection, ParticleAmountKey, false, out particleAmount));

            float resampleNoiseSize;
            this.errors.AddRange(this.TryGetFloat(DefaultSection, ResampleNoiseSizeKey, false, out resampleNoiseSize));

            string particleGenerator;
            this.errors.Add(this.TryGetString(DefaultSection, ParticleGeneratorKey, false, out particleGenerator));

            string resampler;
            this.errors.Add(this.TryGetString(DefaultSection, ResamplerKey, false, out resampler));

            string noiseGenerator;
            this.errors.Add(this.TryGetString(DefaultSection, NoiseGeneratorKey, false, out noiseGenerator));

            string smoother;
            this.errors.Add(this.TryGetString(DefaultSection, SmootherKey, false, out smoother));

            return ParticleFilterFactory.Create(particleAmount, resampleNoiseSize, this.fieldSize, particleGenerator, resampler, noiseGenerator, smoother);
        }

        private IUserLocalizer CreateUserLocalizer()
        {
            string localizerName;
            this.errors.Add(this.TryGetString(DefaultSection, LocalizerNameKey, false, out localizerName));
            switch (localizerName)
            {
                case "particlefilter":
                    return this.CreateParticleFilter();
                default:
                    this.errors.Add("Localizer name given by user not recognized");
                    break;
            }

            return null;
        }

        private bool GetBool(string key)
        {
            bool res;
            this.errors.AddRange(this.TryGetBool(DefaultSection, key, false, out res));
            return res;
        }
    }
}