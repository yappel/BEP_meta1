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
        /// Initializes a new instance of the <see cref="ModifyScaleState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public ModifyScaleState(StateContext stateContext) : base(stateContext)
        {
        }
    }
}
