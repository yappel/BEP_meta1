// <copyright file="ModifyState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using Meta;
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
        /// The render components that can be adjusted for the outline
        /// </summary>
        private Renderer[] colorRenders;

        /// <summary>
        /// Green outline shade
        /// </summary>
        private Shader greenOutline = Shader.Find("Outlined/Diffuse_G");

        /// <summary>
        /// The default shade, no outline
        /// </summary>
        private Shader defaultShader = Shader.Find("Standard");

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">the game object to modify</param>
        public ModifyState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            this.StateContext.Buttons.DeleteButton.SetActive(true);
            this.StateContext.Buttons.ConfirmButton.SetActive(true);
            this.StateContext.Buttons.TranslateButton.SetActive(true);
            this.StateContext.Buttons.RotateButton.SetActive(true);
            this.StateContext.Buttons.ScaleButton.SetActive(true);
            this.StateContext.Buttons.CopyButton.SetActive(true);
            this.gameObject = gameObject;
            this.colorRenders = gameObject.transform.GetComponentsInChildren<Renderer>();
            this.ChangeOutlineRender(this.greenOutline);
        }

        /// <summary>
        /// Return to the neutral state when the confirm button is pressed.
        /// </summary>
        public override void OnConfirmButton()
        {
            this.ChangeOutlineRender(this.defaultShader);
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Delete the game object and return to the neutral state.
        /// </summary>
        public override void OnDeleteButton()
        {
            UnityEngine.Object.Destroy(this.gameObject);
            this.StateContext.SetState(new NeutralState(this.StateContext));
        }

        /// <summary>
        /// Go to the rotate state.
        /// </summary>
        public override void OnRotateButton()
        {
            this.StateContext.SetState(new ModifyRotateState(this.StateContext, this.gameObject));
        }

        /// <summary>
        /// Return to the object placement state where the object can be moved.
        /// </summary>
        public override void OnTranslateButton()
        {
            this.StateContext.SetState(new ObjectPlacementState(this.StateContext, this.gameObject.transform.position, this.gameObject));
        }

        /// <summary>
        /// Go to the scale state.
        /// </summary>
        public override void OnScaleButton()
        {
            this.StateContext.SetState(new ModifyScaleState(this.StateContext, this.gameObject));
        }

        /// <summary>
        /// Copy the objet and go to the object placing state.
        /// </summary>
        public override void OnCopyButton()
        {
            this.ChangeOutlineRender(this.defaultShader);
            this.StateContext.SetState(new ObjectPlacementState(this.StateContext, this.gameObject.transform.position, "Objects/" + this.gameObject.name.Replace("(Clone)", string.Empty).Trim()));
        }

        /// <summary>
        /// Change the outline color
        /// </summary>
        /// <param name="shader">New outline shade</param>
        private void ChangeOutlineRender(Shader shader)
        {
            for (int i = 0; i < this.colorRenders.Length; i++)
            {
                this.colorRenders[i].material.shader = shader;
            }
        }
    }
}
