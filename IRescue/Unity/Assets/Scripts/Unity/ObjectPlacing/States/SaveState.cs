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
        /// Initializes a new instance of the <see cref="SaveState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public SaveState(StateContext stateContext) : base(stateContext)
        {
        }
    }
}
