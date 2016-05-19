// <copyright file="ObjectPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using UnityEngine;

    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class ObjectPlacementState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location) : base(stateContext)
        {
        }
    }
}
