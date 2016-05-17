// <copyright file="ModifyScaleState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when scaling a selected building.
    /// </summary>
    public class ModifyScaleState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyScaleState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public ModifyScaleState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
