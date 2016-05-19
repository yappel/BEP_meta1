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
            this.pointTime = StopwatchSingleton.Time;
        }

        /// <summary>
        /// Sets the state to an object placement state after pointing for 1.5 second.
        /// </summary>
        /// <param name="position">Position of the building to be placed</param>
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            if (time - this.pointTime > 1500)
            {
                if (time - this.pointTime < 1750)
                {
                    this.StateContext.SetState(new ObjectPlacementState(this.StateContext, position, this.StateContext.SelectedBuilding));
                }
                else
                {
                    this.pointTime = time;
                }
            }
        }

        /// <summary>
        /// Sets the state to modify an object after 1.5 seconds.
        /// </summary>
        /// <param name="gameObject">Object pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
            long time = StopwatchSingleton.Time;
            if (time - this.pointObjectTime > 1500)
            {
                if (time - this.pointObjectTime < 1750)
                {
                    this.StateContext.SetState(new ModifyState(this.StateContext, gameObject));
                }
                else
                {
                    this.pointObjectTime = time;
                }
            }
        }
    }
}
