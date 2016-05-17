// <copyright file="ModifyRotateState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when rotating a selected building.
    /// </summary>
    public class ModifyRotateState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyRotateState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public ModifyRotateState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
