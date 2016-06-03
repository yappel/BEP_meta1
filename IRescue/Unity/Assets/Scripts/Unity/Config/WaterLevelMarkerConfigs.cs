namespace Assets.Scripts.Unity.Config
{
    using System.Collections.Generic;

    using IniParser.Model;

    using UnityEngine;

    public class WaterLevelMarkerConfigs : AbstractMarkerConfigs<WaterMarkerConfig>
    {
        /// <summary>
        /// TODO
        /// </summary>
        private const string DefaultPath = "";

        private const string DistanceToFirstStripeKey = "distancetofirsstripe";

        private const string StripeHeightKey = "stripeheight";

        private const string StripeWidthKey = "stripewidth";

        private const string StripeAmountKey = "stripeamount";

        private static readonly string DefaultUserPath = Application.persistentDataPath + "WaterLevelTrackingConfig.txt";

        private WaterMarkerConfig defaultValues;

        private Dictionary<int, WaterMarkerConfig> waterMarkerConfigs;

        public WaterLevelMarkerConfigs()
            : this(DefaultUserPath)
        {
        }

        public WaterLevelMarkerConfigs(string path)
            : base(path, DefaultPath)
        {
        }

        protected override WaterMarkerConfig CreateMarkerConfig(string sectionName)
        {
            WaterMarkerConfig newconfig = new WaterMarkerConfig();
            this.TryGetDouble(sectionName, DistanceToFirstStripeKey, false, out newconfig.DistanceToFirstStripe);
            this.TryGetDouble(sectionName, StripeHeightKey, false, out newconfig.StripeHeight);
            this.TryGetDouble(sectionName, StripeWidthKey, false, out newconfig.StripeWidth);
            this.TryGetDouble(sectionName, StripeAmountKey, false, out newconfig.StripeAmount);
            return newconfig;
        }
    }

    public struct WaterMarkerConfig
    {
        public double DistanceToFirstStripe;

        public double StripeHeight;

        public double StripeWidth;

        public double StripeAmount;
    }
}