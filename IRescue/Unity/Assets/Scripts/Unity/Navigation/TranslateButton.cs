// <copyright file="TranslateButton.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Unity.Navigation
{
    using Scripts.Unity.ObjectPlacing;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// This code should be coupled to the Translate button prefab
    /// </summary>
    public class TranslateButton : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Event launched when the coupled button is pressed.
        /// </summary>
        /// <param name="eventData">data of the event when the button is pressed</param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            GameObject.FindObjectOfType<StateController>().ModifyTranslateButtonEvent(eventData);
        }
    }
}