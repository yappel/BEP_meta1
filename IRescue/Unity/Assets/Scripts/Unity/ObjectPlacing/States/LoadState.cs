﻿// <copyright file="LoadState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Meta;
    using UnityEngine;
    using UnityEngine.UI;    
    
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
        /// The height of an entry which displays the file name which can be clicked.
        /// </summary>
        private const int EntryHeight = 80;

        /// <summary>
        /// The width of the panel which shows the parts of the content
        /// </summary>
        private const int FrameWidth = 416;

        /// <summary>
        /// The height of the panel which shows the parts of the content
        /// </summary>
        private const int FrameHeight = 350;

        /// <summary>
        /// The padding between the load entries
        /// </summary>
        private const int Padding = 10;

        /// <summary>
        /// The scroll pane with the load files.
        /// </summary>
        private GameObject scollButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public LoadState(StateContext stateContext) : base(stateContext)
        {
            this.InitButton("ConfirmButton", () => this.OnConfirmButton());
            this.InitButton("BackButton", () => this.OnBackButton());
            this.scollButton = this.InitLoadScrollPane(EntryHeight, FrameWidth, FrameHeight, Padding);
        }

        /// <summary>
        /// Gets or sets the path to the save file if something is loaded.
        /// </summary>
        public string SaveFilePath { get; set; }

        /// <summary>
        /// Load the game and return to the neutral state
        /// </summary>
        public void OnConfirmButton()
        {
            if (this.SaveFilePath != null && this.CanSwitchState())
            {
                try
                {
                    if (!this.SaveFilePath.Equals(this.StateContext.SaveFilePath))
                    {
                        this.DestroyObjects();
                        this.LoadGame();
                        this.StateContext.SaveFilePath = this.SaveFilePath;
                        this.StateContext.SetState(new NeutralState(this.StateContext));
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Destroy the scroll button
        /// </summary>
        public override void DiscardState()
        {
            base.DiscardState();
            UnityEngine.Object.Destroy(this.scollButton);
        }

        /// <summary>
        /// Return to the neutral state.
        /// </summary>
        public void OnBackButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Load a game file, place all objects in the game
        /// </summary>
        private void LoadGame()
        {
            this.LoadObjects(SaveFile + this.SaveFilePath + ".xml", GameObject.FindObjectOfType<GroundPlane>().transform);
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
                newObject.AddComponent<BuildingPlane>();
                MeshRenderer[] meshes = newObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < meshes.Length; i++)
                {
                    meshes[i].material.renderQueue = 3000;
                }

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

        /// <summary>
        /// Initialized the load select frame
        /// </summary>
        /// <param name="entryHeight">the height of an entry</param>
        /// <param name="frameWidth">The width of the frame of the load scroll panel in the prefab</param>
        /// <param name="frameHeight">The height of the frame of the load scroll panel in the prefab</param>
        /// <param name="padding">the padding between entries</param>
        /// <returns>The Object select frame game object</returns>
        private GameObject InitLoadScrollPane(int entryHeight, int frameWidth, int frameHeight, int padding)
        {
            GameObject scollButton = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/LoadScrollButton"));
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\\Saves\\");
            FileSystemInfo[] files = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\\Saves\\").GetFileSystemInfos();
            Array.Sort<FileSystemInfo>(files, delegate(FileSystemInfo a, FileSystemInfo b) { return b.LastWriteTime.CompareTo(a.LastWriteTime); });
            GameObject scrollViewVertical = scollButton.transform.GetChild(0).GetChild(0).gameObject;
            GameObject content = scrollViewVertical.transform.GetChild(0).gameObject;
            float height = (files.Length * (entryHeight + padding)) + (2 * padding);
            this.SetRectTransform(
                content.GetComponent<RectTransform>(),
                new Vector3(0, (0.5f * frameHeight) - height),
                new Vector2(0, height));
            GameObject scrollViewEntry = content.transform.GetChild(0).gameObject;
            for (int i = 0; i < files.Length; i++)
            {
                this.AddLoadEntry(GameObject.Instantiate(scrollViewEntry), content.transform, frameWidth, entryHeight, files[i].FullName, i, height, padding);
            }

            UnityEngine.Object.Destroy(scrollViewEntry);
            scollButton.transform.SetParent(ButtonWrapper.Wrapper, false);
            return scollButton;
        }

        /// <summary>
        /// Edit a rectangle transform
        /// </summary>
        /// <param name="transform">the rectangle transform to modify</param>
        /// <param name="localPosition">the new local position</param>
        /// <param name="sizeDelta">the new size delta</param>
        private void SetRectTransform(RectTransform transform, Vector3 localPosition, Vector2 sizeDelta)
        {
            transform.localPosition = localPosition;
            transform.sizeDelta = sizeDelta;
        }

        /// <summary>
        /// Add an entry to the load scroll pane
        /// </summary>
        /// <param name="entry">the new entry</param>
        /// <param name="parent">the parent to set, the content of the scroll pane</param>
        /// <param name="frameWidth">the width of the scroll pane</param>
        /// <param name="entryHeight">the height of the entry</param>
        /// <param name="path">the path to the file</param>
        /// <param name="i">the index of the call</param>
        /// <param name="contentSize">The height of the content wrapper</param>
        /// <param name="padding">the padding between entries</param>
        private void AddLoadEntry(GameObject entry, Transform parent, int frameWidth, int entryHeight, string path, int i, float contentSize, int padding)
        {
            entry.name = Path.GetFileName(path).Replace(".xml", string.Empty);
            entry.transform.GetChild(0).GetComponent<Text>().text = File.GetLastWriteTime(path).ToString();
            entry.transform.GetChild(1).GetComponent<Text>().text = entry.name;
            entry.transform.SetParent(parent);
            entry.transform.localPosition = new Vector3(0, (i * (-entryHeight - padding)) - ((0.5f * entryHeight) + (1.5f * padding)) + contentSize);
            entry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            entry.AddComponent<LoadSelector>().Init(this);
        }
    }
}