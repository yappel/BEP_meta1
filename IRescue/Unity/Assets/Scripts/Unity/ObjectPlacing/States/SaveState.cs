// <copyright file="SaveState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using System;
    using System.IO;
    using System.Xml;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// State when saving the game.
    /// </summary>
    public class SaveState : AbstractState
    {
        /// <summary>
        /// The folder where the save files are located from Unity/
        /// </summary>
        private const string SaveFile = "Saves/";

        /// <summary>
        /// The game object which displays the save name
        /// </summary>
        private GameObject saveStringInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveState"/> class.
        /// If a save path has already been set, the class only saved the game but does not initialize
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        /// <param name="saveOnly">Default is false, set to true if you only want to save the game and now switch state. IMPORTANT: The state will not initialize</param>
        public SaveState(StateContext stateContext, bool saveOnly = false) : base(stateContext)
        {
            if (saveOnly && this.StateContext.SaveFilePath != null)
            {
                this.SaveGame(SaveFile + this.StateContext.SaveFilePath + ".xml");
            }
            else
            {
                this.InitButton("SaveButton", () => this.OnSaveButton());
                this.InitButton("BackButton", () => this.OnBackButton());
                this.saveStringInput = this.InitTextPane("TextInput", DateTime.Now.ToString("yyyyMMddHHmm"));
            }
        }

        /// <summary>
        /// Return to the neutral state
        /// </summary>
        public void OnBackButton()
        {
            if (this.CanSwitchState())
            {
                Meta.MetaKeyboard.Instance.enabled = false;
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Save the game and return to the neutral state
        /// </summary>
        public void OnSaveButton()
        {
            if (this.CanSwitchState())
            {
                // TODO not enabled but hide only
                Meta.MetaKeyboard.Instance.enabled = false;
                try
                {
                    string saveName = this.saveStringInput.GetComponentInChildren<Text>().text.Replace(" ", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty);
                    this.SaveGame(SaveFile + saveName + ".xml");
                    this.StateContext.SaveFilePath = saveName;
                    this.StateContext.SetState(new NeutralState(this.StateContext));
                }
                catch (Exception)
                {
                    this.saveStringInput.GetComponentInChildren<Text>().text = "Invalid name, try using a different one!";
                }
            }
        }

        /// <summary>
        /// Write all object game objects to a file
        /// </summary>
        /// <param name="path">The path to which will be saved, should contain full path and end with .xml</param>
        private void SaveGame(string path)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\\Saves\\");
            XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("objects");
            foreach (BuildingPlane building in GameObject.FindObjectsOfType<BuildingPlane>())
            {
                this.WriteObject(writer, "Objects/" + building.name.Replace("(Clone)", string.Empty).Trim(), building.transform.localPosition, building.transform.localEulerAngles, building.transform.localScale);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            this.StateContext.SaveFilePath = path.Replace(".xml", string.Empty);
        }

        /// <summary>
        /// Write an object to the xml file.
        /// </summary>
        /// <param name="writer">the xml writer</param>
        /// <param name="objectName">name of the prefab name that is the origin of the game object</param>
        /// <param name="position">the local position of the object</param>
        /// <param name="orientation">the local orientation for the object</param>
        /// <param name="scale">The local scale of the object</param>
        private void WriteObject(XmlTextWriter writer, string objectName, Vector3 position, Vector3 orientation, Vector3 scale)
        {
            writer.WriteStartElement("object");
            writer.WriteStartElement("path");
            writer.WriteString(objectName);
            writer.WriteEndElement();
            writer.WriteStartElement("position");
            this.WriteVector(writer, position);
            writer.WriteEndElement();
            writer.WriteStartElement("orientation");
            this.WriteVector(writer, orientation);
            writer.WriteEndElement();
            writer.WriteStartElement("scale");
            this.WriteVector(writer, scale);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write the vector of the object to the xml file
        /// </summary>
        /// <param name="writer">the xml writer</param>
        /// <param name="vector">the vector to write to the xml</param>
        private void WriteVector(XmlTextWriter writer, Vector3 vector)
        {
            writer.WriteStartElement("x");
            writer.WriteString(vector.x.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("y");
            writer.WriteString(vector.y.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("z");
            writer.WriteString(vector.z.ToString());
            writer.WriteEndElement();
        }
    }
}
