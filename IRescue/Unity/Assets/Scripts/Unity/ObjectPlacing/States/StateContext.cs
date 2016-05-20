// <copyright file="StateContext.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    ///  Context for keeping track of the current states.
    /// </summary>
    public class StateContext : MonoBehaviour
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext"/> class.
        /// </summary>
        public StateContext()
        {
            this.CurrentState = new NeutralState(this);
        }

        /// <summary>
        /// Gets the currentState.
        /// </summary>
        public AbstractState CurrentState { get; private set; }

        /// <summary>
        /// Sets the currentState.
        /// </summary>
        /// <param name="newState">The new current state</param>
        public void SetState(AbstractState newState)
        {
            this.CurrentState = newState;
        }
    }
}
