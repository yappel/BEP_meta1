namespace Assets.Scripts.Unity.Config
{
    using System;
    using System.Collections.Generic;

    using IniParser;
    using IniParser.Exceptions;
    using IniParser.Model;

    using UnityEngine;

    using Vector3 = IRescue.Core.DataTypes.Vector3;

    public abstract class AbstractConfigs
    {
        protected IniData userConfig;

        private IniData defaultConfig;

        protected AbstractConfigs(string path, string defaultPath)
        {
            FileIniDataParser parser = new FileIniDataParser();
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Copy(defaultPath, path);
            }

            this.userConfig = parser.ReadFile(path);
            this.defaultConfig = parser.ReadFile(defaultPath);
            Debug.Log(path);
            foreach (SectionData section in this.user.Sections)
            {
                Debug.Log(section.SectionName);
                foreach (var key in section.Keys)
                {
                    Debug.Log(section.SectionName + ":" + key.KeyName);
                }
            }
        }

        protected virtual List<string> TryGetBool(string section, string key, bool forceDefault, out bool parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errors = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (bool.TryParse(toparse, out parsed))
            {
                return errors;
            }

            this.TryGetDefaultValue(section, key, out toparse);
            parsed = bool.Parse(toparse);
            errors.Add(this.GetErrorMessage(defaultUsed, typeof(bool), section, key, toparse));
            return errors;
        }

        protected virtual List<string> TryGetDouble(string section, string key, bool forceDefault, out double parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errors = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (double.TryParse(toparse, out parsed))
            {
                return errors;
            }


            this.TryGetDefaultValue(section, key, out toparse);
            parsed = double.Parse(toparse);
            errors.Add(this.GetErrorMessage(defaultUsed, typeof(double), section, key, toparse));
            return errors;
        }

        protected virtual List<string> TryGetFloat(string section, string key, bool forceDefault, out float parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errors = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (float.TryParse(toparse, out parsed))
            {
                return errors;
            }

            this.TryGetDefaultValue(section, key, out toparse);
            parsed = float.Parse(toparse);
            errors.Add(this.GetErrorMessage(defaultUsed, typeof(float), section, key, toparse));
            return errors;
        }

        protected virtual List<string> TryGetInt(string section, string key, bool forceDefault, out int parsed)
        {
            string toparse;
            bool defaultUsed;
            List<string> errors = new List<string> { this.GetToParse(section, key, forceDefault, out toparse, out defaultUsed) };
            if (int.TryParse(toparse, out parsed))
            {
                return errors;
            }

            this.TryGetDefaultValue(section, key, out toparse);
            parsed = int.Parse(toparse);
            errors.Add(this.GetErrorMessage(defaultUsed, typeof(int), section, key, toparse));
            return errors;
        }

        protected string TryGetString(string section, string key, bool forceDefault, out string parsed)
        {
            bool defaultUsed;
            return this.GetToParse(section, key, forceDefault, out parsed, out defaultUsed);
        }

        private string TryGetValue(string section, string key, out string value, out bool defaultUsed)
        {
            string sectionKeyPair = section + this.userConfig.SectionKeySeparator + key;
            if (this.userConfig.TryGetKey(sectionKeyPair, out value))
            {
                defaultUsed = false;
                return string.Empty;
            }

            defaultUsed = true;
            this.TryGetDefaultValue(section, key, out value);
            return string.Format("Could not find section and key pair '{0}:{1}' in the config file. Default value is used.", section, key);
        }

        protected virtual List<string> TryGetVector3(string section, string keyx, string keyy, string keyz, bool forceDefault, out Vector3 parsed)
        {
            float[] xyz = new float[3];
            List<string> errors = new List<string>();
            errors.AddRange(this.TryGetFloat(section, keyx, forceDefault, out xyz[0]));
            errors.AddRange(this.TryGetFloat(section, keyy, forceDefault, out xyz[1]));
            errors.AddRange(this.TryGetFloat(section, keyz, forceDefault, out xyz[2]));
            parsed = new Vector3(xyz);

            return errors;
        }

        protected string GetToParse(string section, string key, bool forceDefault, out string toparse, out bool defaultUsed)
        {
            if (forceDefault)
            {
                defaultUsed = true;
                this.TryGetDefaultValue(section, key, out toparse);
                return string.Empty;
            }

            return this.TryGetValue(section, key, out toparse, out defaultUsed);
        }

        protected string GetErrorMessage(bool defaultUsed, Type type, string section, string key, string value)
        {
            if (defaultUsed)
            {
                return string.Format(
                    "The value in the default config file for the key '{0}:{1}' could not be parsed to a {2}", section, key, type);
            }

            return string.Format("Could not parse {0} to a {1}.", value, type);
        }

        private void TryGetDefaultValue(string section, string key, out string value)
        {
            string sectionKeyPair = section + this.defaultConfig.SectionKeySeparator + key;
            if (!this.defaultConfig.TryGetKey(sectionKeyPair, out value))
            {
                throw new WrongDefaultConfigFileException(string.Format("Could not find section and key pair '{0}:{1}' in the default config file.", section, key));
            }
        }
    }
}