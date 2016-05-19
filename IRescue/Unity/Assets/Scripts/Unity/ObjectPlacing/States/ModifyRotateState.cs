// <copyright file="ModifyRotateState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Assets.Unity.Navigation;
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
        /// <param name="stateContext">the state context that keeps track of the states</param>
        /// <param name="gameObject">The object that will be rotated</param>
        public ModifyRotateState(StateContext stateContext, GameObject gameObject)
        {
            this.stateContext = stateContext;
            this.gameObject = gameObject;
            this.gameObject.GetComponent<MetaBody>().rotateObjectOnGrab = true;
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/BackButton"));
        }

        /// <summary>
        /// Return to the modify state
        /// </summary>
        public override void OnBackButton()
        {
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<BackButton>().transform.root.gameObject);
            this.gameObject.GetComponent<MetaBody>().rotateObjectOnGrab = false;
            this.stateContext.SetState(new ModifyState(this.stateContext, this.gameObject));
        }
    }
}
