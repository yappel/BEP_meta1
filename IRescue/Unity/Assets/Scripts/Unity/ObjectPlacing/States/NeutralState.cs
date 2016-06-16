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
        /// The time you need to point before placing a building
        /// </summary>
        private const int TimeToPoint = 3000;

        /// <summary>
        /// The time that is being pointed.
        /// </summary>
        private long pointTime;

        /// <summary>
        /// The time that is being pointed towards a building
        /// </summary>
        private long pointObjectTime;

        /// <summary>
        /// Boolean if the user was pointing this iteration.
        /// </summary>
        private bool hasPointed = false;

        /// <summary>
        /// Amount of cycles not pointed
        /// </summary>
        private int notPointCount = 0;

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
            this.pointTime = StopwatchSingleton.Time;
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
        /// Sets the state to an object placement state after pointing for 3 second.
        /// </summary>
        /// <param name="position">Position of the building to be placed</param>
        /// <param name="handType">The hand that is pointing</param>
        public override void OnPoint(Vector3 position, HandType handType)
        {
            this.hasPointed = true;
            long time = StopwatchSingleton.Time;
            this.pointObjectTime = 0;
            if (time - this.pointTime > TimeToPoint + 250)
            {
                this.pointTime = time;
            }
            else if (time - this.pointTime > TimeToPoint)
            {
                Debug.Log(handType);
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
            this.hasPointed = true;
            long time = StopwatchSingleton.Time;
            this.pointTime = 0;
            if (time - this.pointObjectTime > TimeToPoint + 250)
            {
                this.pointObjectTime = time;
            } 
            else if (time - this.pointObjectTime > TimeToPoint)
            {
                this.StateContext.SetState(new ModifyState(this.StateContext, gameObject));
            }
        }

        /// <summary>
        /// If the user was not pointing, reset the timers
        /// </summary>
        public override void RunLateUpdate()
        {
            if (!this.hasPointed)
            {
                this.notPointCount++;
            }
            else
            {
                this.notPointCount = 0;
            }

            if (this.notPointCount > 30)
            {
                this.pointTime = 0;
                this.pointObjectTime = 0;
            }

            this.hasPointed = false;
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
