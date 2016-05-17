// <copyright file="SaveState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when saving the game.
    /// </summary>
    public class SaveState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public SaveState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
