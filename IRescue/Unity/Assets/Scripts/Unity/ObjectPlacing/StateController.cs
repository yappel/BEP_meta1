// <copyright file="StateController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing
{
    using States;
    using UnityEngine;

    /// <summary>
    ///  Controller for holding track of the gestures and states.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Method called on start. Initialize the StateContext
        /// </summary>
        public void Start()
        {
            this.stateContext = new StateContext();
        }

        /// <summary>
        /// Method called on every frame update.
        /// </summary>
        public void Update()
        {
            this.stateContext.CurrentState.RunUpdate();
            this.PointEvent();
        }
        
        /// <summary>
        /// Method for determining if a point event occurred.
        /// </summary>
        private void PointEvent()
        {
            bool pointing = false;
            if (pointing == true)
            {
                this.stateContext.CurrentState.OnPoint(new Vector3(0, 0, 0));
            }
        }
    }
}
