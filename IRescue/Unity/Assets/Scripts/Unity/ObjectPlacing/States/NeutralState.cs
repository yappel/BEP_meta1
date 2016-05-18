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
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// The time that is being pointed.
        /// </summary>
        long pointTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeutralState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public NeutralState(StateContext stateContext)
        {
            this.stateContext = stateContext;
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
                    this.stateContext.SetState(new ObjectPlacementState(this.stateContext, position));
                }
                else
                {
                    this.pointTime = time;
                }
            }
        }

        /// <summary>
        /// Sets the state to modify an object.
        /// </summary>
        /// <param name="gameObject"></param>
        public override void OnPoint(GameObject gameObject)
        {
            //this.stateContext.SetState(new ModifyState(this.stateContext, gameObject));
        }
    }
}
