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
        /// Initializes a new instance of the <see cref="BuildingPlacementState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public BuildingPlacementState(StateContext stateContext) : base(stateContext)
        {
        }
    }
}
