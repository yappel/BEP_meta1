﻿// <copyright file="AbstractState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Meta;
    using UnityEngine;

    /// <summary>
    /// Abstract class for the implemented States.
    /// </summary>
    public abstract class AbstractState
    {
        /// <summary>
        /// Method when a grab event has occurred. A closed fist.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a grab gesture</param>
        public virtual void OnGrab(HandType hand)
        {
        }

        /// <summary>
        /// Method when a hand is open. An open hand.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a pinch gesture</param>
        public virtual void OnOpen(HandType hand)
        {
        }

        /// <summary>
        /// Method when a pinch event has occurred. A pinch using the thumb and index finger.
        /// </summary>
        /// <param name="hand">The hand(s) which perform a pinch gesture</param>
        public virtual void OnPinch(HandType hand)
        {
        }

        /// <summary>
        /// Gets the stateContext.
        /// Coupled state context which keeps track of the current active state.
        /// </summary>
        public StateContext StateContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        protected AbstractState(StateContext stateContext)
        {
            this.StateContext = stateContext;
        }

        /// <summary>
        /// Method when a point event has occurred
        /// </summary>
        /// <param name="position">The position pointed towards</param>
        public virtual void OnPoint(Vector3 position)
        {
        }

        /// <summary>
        /// Method when a point event has occurred towards a building. A single finger fully extended.
        /// </summary>
        /// <param name="gameObject">The gameObject pointed at</param>
        public virtual void OnPoint(GameObject gameObject)
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
