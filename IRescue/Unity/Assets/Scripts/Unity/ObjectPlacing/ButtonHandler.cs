// <copyright file="ButtonHandler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using Assets.Unity.Navigation;
    using UnityEngine;

    /// <summary>
    ///  Controller for holding track of the gestures and states.
    /// </summary>
    public class ButtonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonHandler"/> class
        /// </summary>
        public ButtonHandler()
        {
            this.ConfirmButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/ConfirmButton")));
            this.DeleteButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/DeleteButton")));
            this.TranslateButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/TranslateButton")));
            this.RotateButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/RotateButton")));
            this.ScaleButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/ScaleButton")));
            this.BackButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/BackButton")));
            this.ResetButtons();
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
            this.ScaleButton.SetActive(false);
            this.RotateButton.SetActive(false);
            this.TranslateButton.SetActive(false);
            this.DeleteButton.SetActive(false);
            this.ConfirmButton.SetActive(false);
            this.BackButton.SetActive(false);
        }

        /// <summary>
        /// Return the button in the MGUI canvas.
        /// </summary>
        /// <param name="wrapper">The root of the game object</param>
        /// <returns>The MGUI.Button game objects</returns>
        private GameObject GetButton(GameObject wrapper)
        {
            return wrapper.transform.GetChild(0).GetChild(0).gameObject;
        }
    }
}
