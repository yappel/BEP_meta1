// <copyright file="ModifyTranslateState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    /// State when translating a selected building.
    /// </summary>
    public class ModifyTranslateState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTranslateState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">The game object to be translated</param>
        public ModifyTranslateState(StateContext stateContext, GameObject gameObject)
        {
            this.stateContext = stateContext;
            this.gameObject = gameObject;
        }
    }
}
