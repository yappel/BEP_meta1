// <copyright file="ModifyScaleState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Assets.Unity.Navigation;
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when scaling a selected building.
    /// </summary>
    public class ModifyScaleState : AbstractState
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
        /// Initializes a new instance of the <see cref="ModifyScaleState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">The game object to be scaled</param>
        public ModifyScaleState(StateContext stateContext, GameObject gameObject)
        {
            this.stateContext = stateContext;
            this.gameObject = gameObject;
            this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab = true;
        }

        /// <summary>
        /// Return to the modify state
        /// </summary>
        public override void OnBackButton()
        {
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<BackButton>().transform.root.gameObject);
            this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab = false;
            this.stateContext.SetState(new ModifyState(this.stateContext, this.gameObject));
        }
    }
}
