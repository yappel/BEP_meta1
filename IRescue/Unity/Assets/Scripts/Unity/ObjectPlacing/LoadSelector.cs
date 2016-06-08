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
        /// load state which listens to this button
        /// </summary>
        private LoadState state;

        /// <summary>
        /// Set the controller
        /// </summary>
        /// <param name="state">the state which can load a game</param>
        public void Init(LoadState state)
        {
            this.state = state;
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
            this.state.SaveFilePath = this.name;
        }
    }
}