// <copyright file="NeutralState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
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
        /// Initializes a new instance of the <see cref="NeutralState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public NeutralState(StateContext stateContext) : base(stateContext)
        {
            this.InitButton("ToggleButton", () => this.OnToggleButton());
            this.InitButton("SaveButton", () => this.OnSaveButton());
            this.InitButton("LoadButton", () => this.OnLoadButton());
            this.pointTime = StopwatchSingleton.Time;
        }

        /// <summary>
        /// Go to the save state
        /// </summary>
        public void OnSaveButton()
        {
            Debug.Log(this.StateContext.SaveFilePath);
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
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            this.pointObjectTime = 0;
            if (time - this.pointTime > TimeToPoint + 250)
            {
                this.pointTime = time;
            }
            else if (time - this.pointTime > TimeToPoint)
            {
                this.StateContext.SetState(new ObjectPlacementState(this.StateContext, position, this.StateContext.SelectedBuilding));
            }
        }

        /// <summary>
        /// Sets the state to modify an object after 3 seconds.
        /// </summary>
        /// <param name="gameObject">Object pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
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
