// <copyright file="ButtonHandler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using UnityEngine;
    using UnityEngine.UI;

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
        /// <param name="controller">The state controller which tracks events</param>
        public ButtonHandler(StateController controller)
        {
            this.buttons = new List<GameObject>();

            // Create the confirm button
            this.ConfirmButton = this.AddButton("Prefabs/Buttons/ConfirmButton", () => controller.ConfirmButtonEvent());

            // Create the delete button
            this.DeleteButton = this.AddButton("Prefabs/Buttons/DeleteButton", () => controller.DeleteButtonEvent());

            // Create the translate button
            this.TranslateButton = this.AddButton("Prefabs/Buttons/TranslateButton", () => controller.ModifyTranslateButtonEvent());

            // Create the rotate button
            this.RotateButton = this.AddButton("Prefabs/Buttons/RotateButton", () => controller.ModifyRotateButtonEvent());

            // Create the scale button
            this.ScaleButton = this.AddButton("Prefabs/Buttons/ScaleButton", () => controller.ModifyScaleButtonEvent());

            // Create the backbutton
            this.BackButton = this.AddButton("Prefabs/Buttons/BackButton", () => controller.BackButtonEvent());
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
    }
}
