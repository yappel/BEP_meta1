// <copyright file="ModifyTranslateState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when translating a selected building.
    /// </summary>
    public class ModifyTranslateState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTranslateState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public ModifyTranslateState(StateContext stateContext) : base(stateContext)
        {
        }
    }
}
