// <copyright file="ModifyState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using UnityEngine;

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    ///  State when a building is selected for modification.
    /// </summary>
    public class ModifyState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// The game object that is being modified.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public ModifyState(StateContext stateContext, GameObject gameObject)
        {
            this.stateContext = stateContext;
            this.gameObject = gameObject;
        }
    }
}
