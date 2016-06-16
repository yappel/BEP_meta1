// <copyright file="NeutralState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;

    /// <summary>
    ///  State when building mode is on, but no actions like placing are happening.
    /// </summary>
    public class NeutralState : AbstractState
    {
        /// <summary>
        /// Counter to count cycles
        /// </summary>
        private int counter = 0;

        /// <summary>
        /// Boolean if the user was grabbing during this cycle
        /// </summary>
        private HandType currentGrabPointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeutralState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public NeutralState(StateContext stateContext) : base(stateContext)
        {
            this.InitButton("ToggleButton", () => this.OnToggleButton());
            this.InitButton("SaveButton", () => this.OnSaveButton());
            this.InitButton("LoadButton", () => this.OnLoadButton());
            this.InitButton("RunButton", () => this.OnRunButton());
        }

        /// <summary>
        /// Set the running state as active
        /// </summary>
        public void OnRunButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new RunningState(this.StateContext));
            }
        }

        /// <summary>
        /// Go to the save state
        /// </summary>
        public void OnSaveButton()
        {
            if (this.StateContext.SaveFilePath != null)
            {
                new SaveState(this.StateContext, true);
            }
            else if (this.CanSwitchState())
            {
                this.StateContext.SetState(new SaveState(this.StateContext));
            }
        }

        /// <summary>
        /// Go to the load state
        /// </summary>
        public void OnLoadButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new LoadState(this.StateContext));
            }
        }

        /// <summary>
        /// Go to the object placing state or the modify state
        /// </summary>
        /// <param name="hand">hand that performed the gesture</param>
        public override void OnGrab(HandType hand)
        {
            this.counter = 0;
            this.currentGrabPointer = hand;
        }

        /// <summary>
        /// Sets the state to an object placement state after pointing for 3 second.
        /// </summary>
        /// <param name="position">Position of the building to be placed</param>
        /// <param name="handType">The hand that is pointing</param>
        public override void OnPoint(Vector3 position, HandType handType)
        {
            if (this.CanSwitchState() && this.currentGrabPointer != HandType.UNKNOWN && this.currentGrabPointer != handType)
            {
                this.StateContext.SetState(new ObjectPlacementState(this.StateContext, position, this.StateContext.SelectedBuilding, handType));
            }
        }

        /// <summary>
        /// Sets the state to modify an object after 3 seconds.
        /// </summary>
        /// <param name="gameObject">Object pointed at</param>
        /// <param name="handType">The hand that is pointing</param>
        public override void OnPoint(GameObject gameObject, HandType handType)
        {
            if (this.CanSwitchState() && this.currentGrabPointer != HandType.UNKNOWN && this.currentGrabPointer != handType)
            {
                this.StateContext.SetState(new ModifyState(this.StateContext, gameObject));
            }
        }

        /// <summary>
        /// If the user was not pointing, reset the timers
        /// </summary>
        public override void RunLateUpdate()
        {
            if (this.counter > 0)
            {
                this.currentGrabPointer = HandType.UNKNOWN;
            }

            this.counter++;

            // TODO set the shader here
        }

        /// <summary>
        /// Show to dropdown with objects that can be selected for placement.
        /// </summary>
        public void OnToggleButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new ObjectSelectState(this.StateContext));
            }
        }
    }
}
