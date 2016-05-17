// <copyright file="RunningState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    ///  State when the application is running and no more buildings have to be places.
    /// </summary>
    public class RunningState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public RunningState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }

        /// <summary>
        /// When pointing, the user explodes.
        /// </summary>
        /// <param name="position">location pointed towards</param>
        public new void OnPoint(Vector3 position)
        {
            // Do something
        }
    }
}
