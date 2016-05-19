// <copyright file="BackButton.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Unity.Navigation
{
    using Scripts.Unity.ObjectPlacing;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// This script is coupled to the back button in Unity, which means the user wants to go back to the previous state.
    /// </summary>
    public class BackButton : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Event launched when the linked button is pressed.
        /// </summary>
        /// <param name="eventData">Data about the event of the click and the button</param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            GameObject.FindObjectOfType<StateController>().BackButtonEvent(eventData);
        }
    }
}