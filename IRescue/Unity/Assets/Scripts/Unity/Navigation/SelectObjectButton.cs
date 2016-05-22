// <copyright file="SelectObjectButton.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Unity.Navigation
{
    using Scripts.Unity.ObjectPlacing;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// This script is coupled to the slider to select buildings in Unity.
    /// </summary>
    public class SelectObjectButton : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Event launched when the linked button is pressed.
        /// </summary>
        /// <param name="eventData">Data about the event of the click and the button</param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            GameObject.FindObjectOfType<StateController>().SelectObjectButtonEvent(eventData, this.gameObject.name);
        }
    }
}