﻿// <copyright file="ModifyRotateState.cs" company="Delft University of Technology">
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
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Vector of the original orientation used to lock the x and z orientation.
        /// </summary>
        private Vector3 originalOrientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyRotateState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        /// <param name="gameObject">The object that will be rotated</param>
        public ModifyRotateState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            this.gameObject = gameObject;
            MetaBody mb = gameObject.GetComponent<MetaBody>();
            mb.useDefaultGrabSettings = false;
            mb.grabbable = true;
            mb.grabbableDistance = float.MaxValue;
            mb.moveObjectOnGrab = false;
            mb.rotateObjectOnTwoHandedGrab = true;
            this.originalOrientation = gameObject.transform.localEulerAngles;
            this.InitButton("BackButton", () => this.OnBackButton());
            this.InitTextPane("InfoText", "Rotate");
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
                mb.rotateObjectOnTwoHandedGrab = false;
                this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
            }
        }

        /// <summary>
        /// Set the x and z rotation back to the original.
        /// </summary>
        public override void RunLateUpdate()
        {
            this.gameObject.transform.localEulerAngles = new Vector3(this.originalOrientation.x, this.gameObject.transform.localEulerAngles.y, this.originalOrientation.z);
        }
    }
}
