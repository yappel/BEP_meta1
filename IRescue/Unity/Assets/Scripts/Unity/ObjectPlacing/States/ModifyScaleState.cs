﻿// <copyright file="ModifyScaleState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
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
            MetaBody mb = gameObject.GetComponent<MetaBody>();
            mb.useDefaultGrabSettings = false;
            mb.grabbable = true;
            mb.moveObjectOnGrab = false;
            mb.grabbableDistance = float.MaxValue;
            mb.scaleObjectOnTwoHandedGrab = true;
            this.InitButton("BackButton", () => this.OnBackButton());
            this.InitTextPane("InfoText", "Scale");
        }

        /// <summary>
        /// Return to the modify state
        /// </summary>
        public void OnBackButton()
        {
            if (this.CanSwitchState())
            {
                MetaBody mb = this.gameObject.GetComponent<MetaBody>();
                mb.useDefaultGrabSettings = true;
                mb.grabbableDistance = 0.1f;
                mb.grabbable = false;
                mb.scaleObjectOnTwoHandedGrab = false;
                this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
            }
        }
    }
}
