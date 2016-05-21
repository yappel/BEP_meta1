// <copyright file="ModifyScaleState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Assets.Unity.Navigation;
    using Meta;
    using UnityEngine;

    /// <summary>
    ///  State when scaling a selected building.
    /// </summary>
    public class ModifyScaleState : AbstractState
    {
        /// <summary>
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyScaleState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        /// <param name="gameObject">The game object to be scaled</param>
        public ModifyScaleState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            this.gameObject = gameObject;
            this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab = true;
            this.StateContext.Buttons.BackButton.SetActive(true);
        }

        /// <summary>
        /// Return to the modify state
        /// </summary>
        public override void OnBackButton()
        {
            this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab = false;
            this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
        }
    }
}
