// <copyright file="LoadSelector.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using States;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Class for the buttons in the object load pane
    /// </summary>
    public class LoadSelector : MonoBehaviour, IPointerDownHandler
    {
        /// <summary>
        /// Controller to call the event
        /// </summary>
        private StateContext controller;

        /// <summary>
        /// Set the controller
        /// </summary>
        /// <param name="controller">gesture controller that will receive the event call</param>
        public void Init(StateContext controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Event when the button is clicked
        /// </summary>
        /// <param name="pointerEvent">data of the button click</param>
        public void OnPointerDown(PointerEventData pointerEvent)
        {
            foreach (Transform child in this.transform.parent.transform)
            {
                child.GetChild(2).GetComponent<Image>().color = Color.white;
            }

            this.transform.GetChild(2).GetComponent<Image>().color = Color.yellow;
            this.controller.SaveFilePath = this.name;
        }
    }
}