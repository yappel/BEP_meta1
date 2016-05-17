// <copyright file="BuildingPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class BuildingPlacementState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingPlacementState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public BuildingPlacementState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
