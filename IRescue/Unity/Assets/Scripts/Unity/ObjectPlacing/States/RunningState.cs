// <copyright file="RunningState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    ///  State when the application is running and no more buildings have to be places.
    /// </summary>
    public class RunningState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunningState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public RunningState(StateContext stateContext) : base(stateContext)
        {
            this.InitButton("ConfigButton", () => this.OnConfigButton());
        }

        /// <summary>
        /// Go to the config state
        /// </summary>
        private void OnConfigButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new ConfigState(this.StateContext));
            }
        }
    }
}
