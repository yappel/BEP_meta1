namespace Assets.Scripts.Unity.Config
{
    using System.Collections.Generic;

    using IniParser.Model;

    public abstract class AbstractMarkerConfigs<T> : AbstractConfigs
    {
        private readonly Dictionary<int, T> markerConfigs;

        private T defaultValues;

        public AbstractMarkerConfigs(string path, string defaultPath)
            : base(path, defaultPath)
        {
            this.markerConfigs = new Dictionary<int, T>();
            foreach (SectionData sectionData in this.userConfig.Sections)
            {
                this.ProcessSectionData(sectionData);
            }
        }

        /// <summary>
        /// Gets the configurations of a marker.
        /// </summary>
        /// <param name="id">The id of the marker to get the information of.</param>
        /// <returns>The configuration of the marker.</returns>
        public T GetConfig(int id)
        {
            T res;
            return this.markerConfigs.TryGetValue(id, out res) ? res : this.defaultValues;
        }

        protected abstract T CreateMarkerConfig(string sectionName);

        protected void ProcessSectionData(SectionData sectionData)
        {
            switch (sectionData.SectionName)
            {
                case "default":
                    this.defaultValues = this.CreateMarkerConfig(sectionData.SectionName);
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
    }
}