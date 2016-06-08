namespace Assets.Scripts.Unity.Config
{
    using System.Collections.Generic;

    using IniParser.Model;

    public abstract class AbstractMarkerConfigs<T> : AbstractConfigs
    {
        public Dictionary<int, T> markerConfigs { get; private set; }

        protected List<string> errors;

        public AbstractMarkerConfigs(string path, string defaultPath, out List<string> errors)
            : base(path, defaultPath)
        {
            this.errors = new List<string>();
            this.markerConfigs = new Dictionary<int, T>();
            foreach (SectionData sectionData in this.userConfig.Sections)
            {
                this.ProcessSectionData(sectionData);
            }

            errors = this.errors;
        }

        public T Default { get; private set; }

        /// <summary>
        /// Gets the configurations of a marker.
        /// </summary>
        /// <param name="id">The id of the marker to get the information of.</param>
        /// <returns>The configuration of the marker.</returns>
        public T GetConfig(int id)
        {
            T res;
            return this.markerConfigs.TryGetValue(id, out res) ? res : this.Default;
        }
        protected abstract T CreateMarkerConfig(string sectionName);

        protected void ProcessSectionData(SectionData sectionData)
        {
            switch (sectionData.SectionName)
            {
                case "default":
                    this.Default = this.CreateMarkerConfig(sectionData.SectionName);
                    break;
                default:
                    int markerid;
                    if (int.TryParse(sectionData.SectionName, out markerid))
                    {
                        T newconfig = this.CreateMarkerConfig(sectionData.SectionName);
                        this.markerConfigs.Add(markerid, newconfig);
                    }

                    break;
            }
        }

        protected override List<string> TryGetFloat(string section, string key, bool forceDefault, out float parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errormessages = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (float.TryParse(toparse, out parsed))
            {
                return errormessages;
            }

            this.GetToParse("default", key, forceDefault, out toparse, out defaultUsed);
            parsed = float.Parse(toparse);
            return errormessages;
        }

        protected override List<string> TryGetDouble(string section, string key, bool forceDefault, out double parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errormessages = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (double.TryParse(toparse, out parsed))
            {
                return errormessages;
            }

            this.GetToParse("default", key, forceDefault, out toparse, out defaultUsed);
            parsed = double.Parse(toparse);
            return errormessages;
        }
    }
}