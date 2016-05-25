// <copyright file="LoadState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using System;
    using System.Globalization;
    using System.Xml;
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when loading a game.
    /// </summary>
    public class LoadState : AbstractState
    {
        /// <summary>
        /// The folder where the save files are located from Unity/
        /// </summary>
        private const string SaveFile = "Saves/";

        /// <summary>
        /// The previous loaded file
        /// </summary>
        private string prevSave;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public LoadState(StateContext stateContext) : base(stateContext)
        {
            this.prevSave = stateContext.SaveFilePath;
            this.StateContext.Buttons.SetActive(new GameObject[] { this.StateContext.Buttons.ConfirmButton, this.StateContext.Buttons.BackButton, this.StateContext.Buttons.LoadScrollButton });
        }

        /// <summary>
        /// Load the game and return to the neutral state
        /// </summary>
        public override void OnConfirmButton()
        {
            try
            {
                if (this.prevSave != this.StateContext.SaveFilePath)
                {
                    this.DestroyObjects();
                    this.LoadGame();
                    this.StateContext.SetState(new NeutralState(this.StateContext));
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Return to the neutral state.
        /// </summary>
        public override void OnBackButton()
        {
            this.StateContext.SaveFilePath = this.prevSave;
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Load a game file, place all objects in the game
        /// </summary>
        private void LoadGame()
        {
            string path = this.StateContext.SaveFilePath;
            this.LoadObjects(SaveFile + path + ".xml", GameObject.FindObjectOfType<GroundPlane>().transform);
        }

        /// <summary>
        ///   Loads the given XML file and parses it to Markers.
        /// </summary>
        /// <param name="path">Path to the xml file</param>
        /// <param name="parent">the transform with has the objects as child (ground plane)</param>
        private void LoadObjects(string path, Transform parent)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNodeList nodeList = xml.SelectNodes("/objects/object");
            foreach (XmlNode node in nodeList)
            {
                GameObject newObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(node.SelectSingleNode("path").InnerText));
                newObject.AddComponent<MetaBody>();
                newObject.transform.parent = parent;
                newObject.transform.localPosition = this.ParseXmlVector(node.SelectSingleNode("position"));
                newObject.transform.localEulerAngles = this.ParseXmlVector(node.SelectSingleNode("orientation"));
                newObject.transform.localScale = this.ParseXmlVector(node.SelectSingleNode("scale"));
            }
        }

        /// <summary>
        /// Parse an xml node to a Vector3.
        /// </summary>
        /// <param name="vector">xml node with x, y and z tags</param>
        /// <returns>Unity Vector3</returns>
        private Vector3 ParseXmlVector(XmlNode vector)
        {
            return new Vector3(
                    float.Parse(vector.SelectSingleNode("x").InnerText, CultureInfo.InvariantCulture),
                    float.Parse(vector.SelectSingleNode("y").InnerText, CultureInfo.InvariantCulture),
                    float.Parse(vector.SelectSingleNode("z").InnerText, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Destroy all places objects to create a fresh scene.
        /// </summary>
        private void DestroyObjects()
        {
            foreach (Transform building in GameObject.FindObjectOfType<GroundPlane>().transform)
            {
                UnityEngine.Object.Destroy(building.gameObject);
            }
        }
    }
}
