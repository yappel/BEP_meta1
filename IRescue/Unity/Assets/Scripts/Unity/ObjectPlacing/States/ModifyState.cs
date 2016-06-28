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
        /// Position of the copy button
        /// </summary>
        private Vector3 copyButtonPosition;

        /// <summary>
        /// Position of the move button
        /// </summary>
        private Vector3 moveButtonPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyState"/> class.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="gameObject">the game object to modify</param>
        public ModifyState(StateContext stateContext, GameObject gameObject) : base(stateContext)
        {
            this.InitButton("DeleteButton", () => this.OnDeleteButton());
            this.InitButton("ConfirmButton", () => this.OnConfirmButton());
            this.moveButtonPosition = this.InitButton("TranslateButton", () => this.OnTranslateButton()).transform.position;
            this.InitButton("RotateButton", () => this.OnRotateButton());
            this.InitButton("ScaleButton", () => this.OnScaleButton());
            this.copyButtonPosition = this.InitButton("CopyButton", () => this.OnCopyButton()).transform.position;
            this.gameObject = gameObject;
            this.colorRenders = gameObject.transform.GetComponentsInChildren<Renderer>();
            this.ChangeOutlineRender(this.greenOutline);
        }

        /// <summary>
        /// Return to the neutral state when the confirm button is pressed.
        /// </summary>
        public void OnConfirmButton()
        {
            if (this.CanSwitchState())
            {
                this.ChangeOutlineRender(this.defaultShader);
                MeshRenderer[] meshes = this.gameObject.gameObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < meshes.Length; i++)
                {
                    meshes[i].material.renderQueue = 3000;
                }

                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Delete the game object and return to the neutral state.
        /// </summary>
        public void OnDeleteButton()
        {
            if (this.CanSwitchState())
            {
                UnityEngine.Object.Destroy(this.gameObject);
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Go to the rotate state.
        /// </summary>
        public void OnRotateButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new ModifyRotateState(this.StateContext, this.gameObject));
            }
        }

        /// <summary>
        /// Return to the object placement state where the object can be moved.
        /// </summary>
        public void OnTranslateButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new ObjectPlacementState(this.StateContext, this.gameObject.transform.position, this.gameObject, this.CheckHandType(this.moveButtonPosition)));
            }
        }

        /// <summary>
        /// Go to the scale state.
        /// </summary>
        public void OnScaleButton()
        {
            if (this.CanSwitchState())
            {
                this.StateContext.SetState(new ModifyScaleState(this.StateContext, this.gameObject));
            }
        }

        /// <summary>
        /// Copy the objet and go to the object placing state.
        /// </summary>
        public void OnCopyButton()
        {
            GameObject newBuilding = UnityEngine.Object.Instantiate<GameObject>(this.gameObject);
            newBuilding.transform.parent = this.gameObject.transform.parent;
            newBuilding.name = this.gameObject.name;
            newBuilding.transform.localScale = this.gameObject.transform.localScale;
            newBuilding.transform.localRotation = this.gameObject.transform.localRotation;
            UnityEngine.Object.Destroy(newBuilding.GetComponent<MetaBody>());
            this.ChangeOutlineRender(this.defaultShader);
            this.StateContext.SetState(new ObjectPlacementState(this.StateContext, this.gameObject.transform.position, newBuilding, this.CheckHandType(this.copyButtonPosition)));
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
