﻿// <copyright file="NeutralState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    ///  State when building mode is on, but no actions like placing are happening.
    /// </summary>
    public class NeutralState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeutralState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        public NeutralState(StateContext stateContext)
        {
            this.stateContext = stateContext;
        }
    }
}
