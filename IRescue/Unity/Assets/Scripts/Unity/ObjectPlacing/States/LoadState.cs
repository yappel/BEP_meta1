// <copyright file="LoadState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    /// <summary>
    /// State when loading a game.
    /// </summary>
    public class LoadState : AbstractState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadState"/> class.
        /// </summary>
        /// <param name="stateContext">The class that keeps track of the current active state</param>
        public LoadState(StateContext stateContext) : base(stateContext)
        {
            this.StateContext.Buttons.ConfirmButton.SetActive(true);
            this.StateContext.Buttons.BackButton.SetActive(true);
            this.StateContext.Buttons.LoadScrollButton.SetActive(true);
        }

        public override void OnConfirmButton()
        {
            // TODO load objects
            this.StateContext.SaveFilePath = "SOMETHING";
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Return to the neutral state.
        /// </summary>
        public override void OnBackButton()
        {
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }
    }
}
