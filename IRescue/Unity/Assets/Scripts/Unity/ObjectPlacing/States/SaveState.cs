// <copyright file="SaveState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;

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
            if (this.StateContext != null)
            {
                this.SaveGame(this.StateContext.SaveFilePath);
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
            else
            {
                this.StateContext.Buttons.BackButton.SetActive(true);
                this.StateContext.Buttons.SaveButton.SetActive(true);
                this.StateContext.Buttons.TextInput.SetActive(true);
                // TODO set textinput text
            }
        }

        /// <summary>
        /// Return to the neutral state
        /// </summary>
        public override void OnBackButton()
        {
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Save the game and return to the neutral state
        /// </summary>
        public override void OnSaveButton()
        {
            // TODO save all buildings to an xml
            try
            {
                this.SaveGame("SOMETHING");
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
            catch (Exception e)
            {
                // TODO show message
            }
        }

        /// <summary>
        /// Write all object gameobjects to a file
        /// </summary>
        /// <param name="path"></param>
        private void SaveGame(string path)
        {
            //TODO



            this.StateContext.SaveFilePath = path;
        }
    }
}
