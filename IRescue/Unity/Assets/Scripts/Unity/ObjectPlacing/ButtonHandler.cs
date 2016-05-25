// <copyright file="ButtonHandler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using System.Collections.Generic;
    using States;
    using UnityEngine;
    using UnityEngine.UI;
    using System.IO;
    using System;
    /// <summary>
    ///  Controller for holding track of the gestures and states.
    /// </summary>
    public class ButtonHandler
    {
        /// <summary>
        /// The location at which the objects are stored.
        /// </summary>
        private const string ObjectPath = "Objects";

        /// <summary>
        /// Path to the preview location
        /// </summary>
        private const string PreviewPath = "Objects/Previews/";

        /// <summary>
        /// Amount of columns allowed in the frame for object selecting
        /// </summary>
        private const int ColumnCount = 3;

        /// <summary>
        /// The width of an entry for the object selecting
        /// </summary>
        private const int EntryWidth = 160;

        /// <summary>
        /// The height of an entry for the object selecting
        /// </summary>
        private const int EntryHeight = 160;

        /// <summary>
        /// Amount of padding for an entry
        /// </summary>
        private const int Padding = 10;

        /// <summary>
        /// Width of the select frame displayed
        /// </summary>
        private const int FrameWidth = 270;

        /// <summary>
        /// The list of all buttons which are set to inactive on every state switch
        /// </summary>
        private List<GameObject> buttons;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonHandler"/> class
        /// </summary>
        /// <param name="context">The state context which tracks the current state</param>
        public ButtonHandler(StateContext context)
        {
            this.buttons = new List<GameObject>();

            // Create the confirm button
            this.ConfirmButton = this.AddButton("Prefabs/Buttons/ConfirmButton", () => context.CurrentState.OnConfirmButton());

            // Create the delete button
            this.DeleteButton = this.AddButton("Prefabs/Buttons/DeleteButton", () => context.CurrentState.OnDeleteButton());

            // Create the translate button
            this.TranslateButton = this.AddButton("Prefabs/Buttons/TranslateButton", () => context.CurrentState.OnTranslateButton());

            // Create the rotate button
            this.RotateButton = this.AddButton("Prefabs/Buttons/RotateButton", () => context.CurrentState.OnRotateButton());

            // Create the scale button
            this.ScaleButton = this.AddButton("Prefabs/Buttons/ScaleButton", () => context.CurrentState.OnScaleButton());

            // Create the backbutton
            this.BackButton = this.AddButton("Prefabs/Buttons/BackButton", () => context.CurrentState.OnBackButton());
            
            // Create the savebutton
            this.SaveButton = this.AddButton("Prefabs/Buttons/SaveButton", () => context.CurrentState.OnSaveButton());

            // Create the loadbutton
            this.LoadButton = this.AddButton("Prefabs/Buttons/LoadButton", () => context.CurrentState.OnLoadButton());

            // Create the textinput
            this.TextInput = this.AddButton("Prefabs/Buttons/TextInput", () => { });

            // Create the load scroll pane
            this.LoadScrollButton = this.AddButton(this.GetButton(this.InitLoadScrollPane(80, 416, context)), () => { });
        }

        /// <summary>
        /// Gets the back button
        /// </summary>
        public GameObject BackButton { get; private set; }

        /// <summary>
        /// Gets the confirm button
        /// </summary>
        public GameObject ConfirmButton { get; private set; }

        /// <summary>
        /// Gets the rotate button
        /// </summary>
        public GameObject RotateButton { get; private set; }

        /// <summary>
        /// Gets the translate button
        /// </summary>
        public GameObject TranslateButton { get; private set; }

        /// <summary>
        /// Gets the scale button
        /// </summary>
        public GameObject ScaleButton { get; private set; }

        /// <summary>
        /// Gets the delete button
        /// </summary>
        public GameObject DeleteButton { get; private set; }

        /// <summary>
        /// Gets the save button
        /// </summary>
        public GameObject SaveButton { get; private set; }

        /// <summary>
        /// Gets the load button
        /// </summary>
        public GameObject LoadButton { get; private set; }

        /// <summary>
        /// Gets the text input button
        /// </summary>
        public GameObject TextInput { get; private set; }

        /// <summary>
        /// Gets the load scroll button
        /// </summary>
        public GameObject LoadScrollButton { get; private set; }

        /// <summary>
        /// Set all buttons to inactive, making them invisible.
        /// </summary>
        public void ResetButtons()
        {
            for (int i = 0; i < this.buttons.Count; i++)
            {
                this.buttons[i].SetActive(false);
            }
        }

        /// <summary>
        /// Reset the load frame panel.
        /// </summary>
        /// <param name="context">he context that keeps track of the state</param>
        public void RefreshLoadPanel(StateContext context)
        {
            this.buttons.Remove(this.LoadScrollButton);
            this.LoadScrollButton = this.AddButton(this.GetButton(this.InitLoadScrollPane(80, 416, context)), () => { });
        }

        /// <summary>
        /// Active a button
        /// </summary>
        /// <param name="gameObject">the button to active</param>
        public void SetActive(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Set multiple buttons to active
        /// </summary>
        /// <param name="gameObjects">array of buttons to active</param>
        public void SetActive(GameObject[] gameObjects)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                this.SetActive(gameObjects[i]);
            }
        }

        /// <summary>
        /// Add a button to the game from a GameObject
        /// </summary>
        /// <param name="button">The initialized button game object</param>
        /// <param name="action">The lambda of the action that should be taken on press</param>
        /// <returns>The GameObject of the linked button that can be get in this method</returns>
        private GameObject AddButton(GameObject button, UnityEngine.Events.UnityAction action)
        {
            if (button.transform.GetComponentInChildren<Button>() != null)
            {
                button.transform.GetComponentInChildren<Button>().onClick.AddListener(action);
            }

            button.SetActive(false);
            this.buttons.Add(button);
            return button;
        }

        /// <summary>
        /// Add a button to the game from a path name.
        /// </summary>
        /// <param name="buttonPrefabName">The path and name of the button prefab</param>
        /// <param name="action">The lambda of the action that should be taken on press</param>
        /// <returns>The GameObject of the linked button that can be get in this method</returns>
        private GameObject AddButton(string buttonPrefabName, UnityEngine.Events.UnityAction action)
        {
            return this.AddButton(this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>(buttonPrefabName))), action);
        }

        /// <summary>
        /// Return the button in the MGUI canvas.
        /// </summary>
        /// <param name="wrapper">The root of the game object</param>
        /// <returns>The MGUI.Button game objects</returns>
        private GameObject GetButton(GameObject wrapper)
        {
            return wrapper.transform.GetChild(0).gameObject;
        }
        
        /// <summary>
        /// Initialized the load select frame
        /// </summary>
        /// <param name="entryHeight">the height of an entry</param>
        /// <param name="padding">the padding of an entry to either sides</param>
        /// <param name="frameWidth">the preferred size of the frame</param>
        /// <returns>The Object select frame game object</returns>
        /// <param name="context">The state context which tracks the current state</param>
        private GameObject InitLoadScrollPane(int entryHeight, int frameWidth, StateContext context)
        {
            GameObject scollButton = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/LoadScrollButton"));
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\\Saves\\");
            FileSystemInfo[] files = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\\Saves\\").GetFileSystemInfos();
            Array.Sort<FileSystemInfo>(files, delegate (FileSystemInfo a, FileSystemInfo b) { return a.LastWriteTime.CompareTo(b.LastWriteTime); });
            GameObject scrollViewVertical = scollButton.transform.GetChild(0).GetChild(0).gameObject;
            GameObject content = scrollViewVertical.transform.GetChild(0).gameObject;
            float height = files.Length * entryHeight;
            this.SetRectTransform(
                content.GetComponent<RectTransform>(),
                new Vector3(0, -height - 40),
                new Vector2(0, height + 40));
            GameObject scrollViewEntry = content.transform.GetChild(0).gameObject;
            for (int i = 0; i < files.Length; i++)
            {
                this.AddLoadEntry(GameObject.Instantiate(scrollViewEntry), content.transform, frameWidth, entryHeight, context, files[i].FullName, i);
            }

            UnityEngine.Object.Destroy(scrollViewEntry);
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
        /// <param name="context">the state context which keeps track of the current state</param>
        /// <param name="path">the path to the file</param>
        /// <param name="i">the index of the call</param>
        private void AddLoadEntry(GameObject entry, Transform parent, int frameWidth, int entryHeight, StateContext context, string path, int i)
        {
            entry.name = Path.GetFileName(path).Replace(".xml", "");
            entry.transform.SetParent(parent);
            entry.transform.GetChild(0).GetComponent<Text>().text = File.GetLastWriteTime(path).ToString();
            entry.transform.GetChild(1).GetComponent<Text>().text = entry.name;
            entry.transform.localPosition = new Vector3(10 - frameWidth / 2, (i + 1) * entryHeight - 20);
            entry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            entry.transform.GetComponentInChildren<Button>().onClick.AddListener(() => this.ClickLoadButton(entry.transform, context, entry.name));
        }

        /// <summary>
        /// Button listener, click the button and highlight the load entry
        /// </summary>
        /// <param name="entry">the object entry in the scroll pane</param>
        /// <param name="controller">the state controller which will receive the event call</param>
        /// <param name="name">the name of the object</param>
        private void ClickLoadButton(Transform entry, StateContext context, string name)
        {
            foreach (Transform child in entry.parent.transform)
            {
                child.GetChild(2).GetComponent<Image>().color = Color.white;
            }

            entry.GetChild(2).GetComponent<Image>().color = Color.yellow;
            context.SaveFilePath = name;
        }
    }
}
