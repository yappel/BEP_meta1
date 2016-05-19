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
        /// The game object that is being modified.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">the game object to modify</param>
        public ModifyState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/ConfirmButton"));
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/DeleteButton"));
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/TranslateButton"));
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/RotateButton"));
            UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Buttons/ScaleButton"));
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Return to the neutral state when the confirm button is pressed.
        /// </summary>
        public override void OnConfirmButton()
        {
            this.DeleteButtons();
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Delete the game object and return to the neutral state.
        /// </summary>
        public override void OnDeleteButton()
        {
            this.DeleteButtons();
            UnityEngine.Object.Destroy(this.gameObject);
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Go to the rotate state.
        /// </summary>
        public override void OnRotateButton()
        {
            this.DeleteButtons();
            this.StateContext.SetState(new ModifyRotateState(this.StateContext, this.gameObject));
        }

        /// <summary>
        /// Return to the object placement state where the object can be moved.
        /// </summary>
        public override void OnTranslateButton()
        {
            this.DeleteButtons();
            this.StateContext.SetState(new ObjectPlacementState(this.StateContext, this.gameObject.transform.position, this.gameObject));
        }

        /// <summary>
        /// Go to the scale state.
        /// </summary>
        public override void OnScaleButton()
        {
            this.DeleteButtons();
            this.StateContext.SetState(new ModifyScaleState(this.StateContext, this.gameObject));
        }

        /// <summary>
        /// Delete all buttons of the screen.
        /// </summary>
        private void DeleteButtons()
        {
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<ConfirmButton>().transform.root.gameObject);
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<DeleteButton>().transform.root.gameObject);
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<RotateButton>().transform.root.gameObject);
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<ScaleButton>().transform.root.gameObject);
            UnityEngine.Object.Destroy(GameObject.FindObjectOfType<TranslateButton>().transform.root.gameObject);
        }
    }
}
