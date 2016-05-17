// <copyright file="BuildingSelectState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when selecting a building that you want to place.
    /// </summary>
    public class BuildingSelectState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingSelectState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public BuildingSelectState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
