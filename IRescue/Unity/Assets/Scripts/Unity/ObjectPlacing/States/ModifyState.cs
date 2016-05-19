// <copyright file="ModifyState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Assets.Unity.Navigation;
    using UnityEngine;

    /// <summary>
    ///  State when a building is selected for modification.
    /// </summary>
    public class ModifyState : AbstractState
    {
        /// <summary>
        /// Coupled state context.
        /// </summary>
        private StateContext stateContext;

        /// <summary>
        /// The game object to modify.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">the game object to modify</param>
        public ModifyState(StateContext stateContext, GameObject gameObject)
        {
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/ConfirmButton"));
            this.stateContext = stateContext;
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Return to the neutral state when the confirm button is pressed.
        /// </summary>
        public override void OnConfirmButton()
        {
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<ConfirmButton>().transform.root.gameObject);
            this.stateContext.SetState(new NeutralState(this.stateContext));
        }
    }
}
