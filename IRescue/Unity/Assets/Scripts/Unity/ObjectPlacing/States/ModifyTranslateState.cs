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
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTranslateState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public ModifyTranslateState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
