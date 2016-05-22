// <copyright file="ObjectSelectState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when selecting a building that you want to place.
    /// </summary>
    public class ObjectSelectState : AbstractState
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSelectState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public ObjectSelectState(StateContext stateContext) : base(stateContext)
        {   
            this.StateContext.Buttons.ToggleButton.SetActive(true);
        }

        /// <summary>
        /// Disable the dropdown and go back to the neutral state
        /// </summary>
        public override void OnToggleButton()
        {
            this.StateContext.SetState(new ObjectSelectState(this.StateContext));
        }
    }
}
