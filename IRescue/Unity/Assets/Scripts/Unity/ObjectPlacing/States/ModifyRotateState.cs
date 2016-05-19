// <copyright file="ModifyRotateState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when rotating a selected building.
    /// </summary>
    public class ModifyRotateState : AbstractState
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
        /// Initializes a new instance of the <see cref="ModifyRotateState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">The object to rotate</param>
        public ModifyRotateState(StateContext stateContext, GameObject gameObject)
        {
            this.stateContext = stateContext;
            this.gameObject = gameObject;
            gameObject.GetComponent<MetaBody>().rotateObjectOnGrab = true;
        }

        /// <summary>
        /// Return to the modify state
        /// </summary>
        public override void CancelButtonEvent()
        {
            gameObject.GetComponent<MetaBody>().rotateObjectOnGrab = false;
            stateContext.SetState(new ModifyState(this.stateContext, this.gameObject));
        }
    }
}
