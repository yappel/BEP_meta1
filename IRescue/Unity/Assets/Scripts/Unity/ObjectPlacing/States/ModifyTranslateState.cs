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
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTranslateState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        /// <param name="gameObject">The game object to be translated</param>
        public ModifyTranslateState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            this.gameObject = gameObject;
        }
    }
}
