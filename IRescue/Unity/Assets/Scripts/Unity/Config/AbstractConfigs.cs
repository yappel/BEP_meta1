namespace Assets.Scripts.Unity.Config
{
    using System;

    using IniParser;
    using IniParser.Exceptions;
    using IniParser.Model;

    using IRescue.Core.DataTypes;

    public abstract class AbstractConfigs
    {
        protected IniData userConfig;

        private IniData defaultConfig;

        protected AbstractConfigs(string path, string defaultPath)
        {
            FileIniDataParser parser = new FileIniDataParser();
            this.userConfig = parser.ReadFile(path);
            this.defaultConfig = parser.ReadFile(defaultPath);
        }

        protected void TryGetBool(string section, string key, bool forceDefault, out bool parsed)
        {
            string toparse;
            bool defaultUsed;
            this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed);
            if (!bool.TryParse(toparse, out parsed))
            {
                this.ThrowParseException(defaultUsed, typeof(bool), section, key, toparse);
            }
        }

        protected void TryGetDouble(string section, string key, bool forceDefault, out double parsed)
        {
            string toparse;
            bool defaultUsed;
            this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed);
            if (!double.TryParse(toparse, out parsed))
            {
                this.ThrowParseException(defaultUsed, typeof(double), section, key, toparse);
            }
        }

        protected void TryGetFloat(string section, string key, bool forceDefault, out float parsed)
        {
            string toparse;
            bool defaultUsed;
            this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed);
            if (!float.TryParse(toparse, out parsed))
            {
                this.ThrowParseException(defaultUsed, typeof(float), section, key, toparse);
            }
        }

        protected void TryGetInt(string section, string key, bool forceDefault, out int parsed)
        {
            string toparse;
            bool defaultUsed;
            this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed);
            if (!int.TryParse(toparse, out parsed))
            {
                this.ThrowParseException(defaultUsed, typeof(int), section, key, toparse);
            }
        }

        protected void TryGetString(string section, string key, bool forceDefault, out string parsed)
        {
            bool defaultUsed;
            this.GetToParse(section, key, forceDefault, out parsed, out defaultUsed);
        }

        private void TryGetValue(string section, string key, out string value, out bool defaultUsed)
        {
            string sectionKeyPair = section + this.userConfig.SectionKeySeparator + key;
            if (this.userConfig.TryGetKey(sectionKeyPair, out value))
            {
                defaultUsed = false;
                return;
            }

            defaultUsed = true;
            this.TryGetDefaultValue(section, key, out value);
            throw new KeyNotFoundException($"Could not find section and key pair '{section}:{key}' in the config file. Default value is used.");
        }

        protected void TryGetVector3(string section, string keyx, string keyy, string keyz, bool forceDefault, out Vector3 parsed)
        {
            float[] xyz = new float[3];
            this.TryGetFloat(section, keyx, forceDefault, out xyz[0]);
            this.TryGetFloat(section, keyy, forceDefault, out xyz[1]);
            this.TryGetFloat(section, keyz, forceDefault, out xyz[2]);
            parsed = new Vector3(xyz);
        }

        private void GetToParse(string section, string key, bool forceDefault, out string toparse, out bool defaultUsed)
        {
            if (forceDefault)
            {
                defaultUsed = true;
                this.TryGetDefaultValue(section, key, out toparse);
            }
            else
            {
                this.TryGetValue(section, key, out toparse, out defaultUsed);
            }
        }

        private void ThrowParseException(bool defaultUsed, Type type, string section, string key, string value)
        {
            if (defaultUsed)
            {
                throw new WrongDefaultConfigFileException($"The value in the default config file for the key '{section}:{key}' could not be parsed to a {type}");
            }

            throw new ParsingException($"Could not parse {value} to a {type}.");
        }

        private void TryGetDefaultValue(string section, string key, out string value)
        {
            string sectionKeyPair = section + this.defaultConfig.SectionKeySeparator + key;
            if (!this.defaultConfig.TryGetKey(sectionKeyPair, out value))
            {
                throw new WrongDefaultConfigFileException($"Could not find section and key pair '{section}:{key}' in the default config file.");
            }
        }
    }
}