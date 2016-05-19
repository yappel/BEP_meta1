// <copyright file="ModifyState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    ///  State when a building is selected for modification.
    /// </summary>
    public class ModifyState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public ModifyState(StateContext stateContext) : base(stateContext)
        {
        }
    }
}
