﻿// <copyright file="AbstractState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    /// Abstract class for the implemented States.
    /// </summary>
    public abstract class AbstractState
    {
        /// <summary>
        /// Method called when the cancel or return button was pressed
        /// </summary>
        public virtual void OnBackButton()
        {
        }

        /// <summary>
        /// Method called when the confirm button was pressed
        /// </summary>
        public virtual void OnConfirmButton()
        {
        }

        /// <summary>
        /// Method when a point event has occurred
        /// </summary>
        /// <param name="position">The position pointed towards</param>
        public virtual void OnPoint(Vector3 position)
        {
        }

        /// <summary>
        /// Run an update on the state.
        /// </summary>
        public virtual void RunUpdate()
        {
        }
    }
}
