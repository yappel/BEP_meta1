// <copyright file="ButtonHandler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using System;
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
            this.ToggleButton = this.GetButton(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/ToggleButton")));
            this.ObjectSelect = this.GetButton(this.InitObjectSelect(3, 160, 160, 10, 270));
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
        /// Gets the toggle button
        /// </summary>
        public GameObject ToggleButton { get; private set; }

        /// <summary>
        /// Gets the delete button
        /// </summary>
        public GameObject DeleteButton { get; private set; }

        /// <summary>
        /// Gets the object select button
        /// </summary>
        public GameObject ObjectSelect { get; private set; }

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
            this.ObjectSelect.SetActive(false);
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

        private GameObject InitObjectSelect(int columnSize, int entryWidth, int entryHeight, int padding, int frameWidth)
        {
            GameObject objectSelect = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/ObjectSelect"));
            GameObject[] objects = Resources.LoadAll<GameObject>(ObjectPath);
            GameObject scrollViewVertical = objectSelect.transform.GetChild(0).GetChild(1).gameObject;
            this.SetRectTransform(scrollViewVertical.GetComponent<RectTransform>(), new Vector3(-80, -35, 0), new Vector2((entryWidth + 2 * padding) * columnSize, frameWidth));
            GameObject content = scrollViewVertical.transform.GetChild(0).gameObject;
            this.SetRectTransform(
                content.GetComponent<RectTransform>(), 
                new Vector3(-frameWidth, -(230 + (entryWidth * (1 + Mathf.Floor((objects.Length / columnSize)))))), 
                new Vector2((entryWidth + 2 * padding) * columnSize, (entryHeight + 2 * padding) * (1 + Mathf.Floor((objects.Length / columnSize)))));
            float deductY = content.GetComponent<RectTransform>().sizeDelta.y;
            GameObject scrollViewEntry = content.transform.GetChild(0).gameObject;
            for (int i  = 0; i < objects.Length; i++)
            {
                this.AddScrollEntry(GameObject.Instantiate(scrollViewEntry), objects[i].name, deductY, content.transform, i, columnSize, entryWidth, entryHeight, padding, frameWidth);
                
            }

            UnityEngine.Object.Destroy(scrollViewEntry);
            return objectSelect;
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
        /// Initialize a scroll entry
        /// </summary>
        /// <param name="entry">The entry game object</param>
        /// <param name="name">name of the object</param>
        /// <param name="deductY">the correction of the local position y of the parent</param>
        /// <param name="parent">the transform to set as parent, should be content</param>
        /// <param name="i">the increment counter to place the entry at the correct row and column</param>
        private void AddScrollEntry(
            GameObject entry, string name, float deductY, Transform parent, int i, int columnSize, int entryWidth, int entryHeight, int padding, int frameWidth)
        {
            entry.transform.SetParent(parent);
            entry.name = name;
            entry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            entry.GetComponent<RectTransform>().localPosition = new Vector3(
                padding + Mathf.Floor(i % columnSize) * (entryWidth + (2 * padding)), 
                -((Mathf.Floor((i / columnSize) * entryHeight) + (entryHeight + (2 * padding)) - deductY)));
            entry.transform.GetComponentInChildren<Text>().text = name;
        }
    }
}
