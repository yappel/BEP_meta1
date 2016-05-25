// <copyright file="SaveState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using System;
    using UnityEngine;
    using System.Xml;
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
        /// Initializes a new instance of the <see cref="SaveState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public SaveState(StateContext stateContext) : base(stateContext)
        {
            if (this.StateContext.SaveFilePath != null)
            {
                this.SaveGame(SaveFile + this.StateContext.SaveFilePath + ".xml");
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
            else
            {
                this.StateContext.Buttons.SetActive(new GameObject[] { this.StateContext.Buttons.BackButton, this.StateContext.Buttons.SaveButton, this.StateContext.Buttons.TextInput });
                this.StateContext.Buttons.TextInput.GetComponentInChildren<Text>().text = DateTime.Now.ToString("yyyyMMddHHmm");
            }
        }

        /// <summary>
        /// Return to the neutral state
        /// </summary>
        public override void OnBackButton()
        {
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Save the game and return to the neutral state
        /// </summary>
        public override void OnSaveButton()
        {
            try
            {
                string saveName = StateContext.Buttons.TextInput.GetComponentInChildren<Text>().text.Replace(" ", "").Replace("\\", "").Replace("/", "");
                this.SaveGame(SaveFile + saveName + ".xml");
                this.StateContext.SaveFilePath = saveName;
                this.StateContext.Buttons.RefreshLoadPanel(this.StateContext);
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
            catch (Exception)
            {
                this.StateContext.Buttons.TextInput.GetComponentInChildren<Text>().text = "Invalid name, try using a different one!";
            }
        }

        /// <summary>
        /// Write all object gameobjects to a file
        /// </summary>
        /// <param name="path"></param>
        private void SaveGame(string path)
        {
            XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("objects");
            foreach (Transform building in GameObject.FindObjectOfType<GroundPlane>().transform)
            {
                this.WriteObject(writer, "Objects/DefaultObject/" + building.name.Replace("(Clone)", "").Trim(), building.localPosition, building.localEulerAngles, building.localScale);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            this.StateContext.SaveFilePath = path;
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
