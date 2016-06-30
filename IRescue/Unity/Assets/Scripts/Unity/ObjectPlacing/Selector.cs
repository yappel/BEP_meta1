// <copyright file="Selector.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using States;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Class for the buttons in the object select pane
    /// </summary>
    public class Selector : MonoBehaviour, IPointerDownHandler
    {
        /// <summary>
        /// Controller to call the event
        /// </summary>
        private ObjectSelectState state;

        /// <summary>
        /// Set the controller
        /// </summary>
        /// <param name="state">the object select state which will receive the event</param>
        public void Init(ObjectSelectState state)
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
                child.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
            }
            
            this.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.yellow;
            this.state.SelectObjectButtonEvent(this.name);
        }
    }
}