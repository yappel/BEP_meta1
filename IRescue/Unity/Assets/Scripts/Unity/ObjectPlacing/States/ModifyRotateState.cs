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
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyRotateState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        /// <param name="gameObject">The object that will be rotated</param>
        public ModifyRotateState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
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
            this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
        }
    }
}
