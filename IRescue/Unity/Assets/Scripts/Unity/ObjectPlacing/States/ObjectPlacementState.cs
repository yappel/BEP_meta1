// <copyright file="ObjectPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class ObjectPlacementState : AbstractState
    {
        /// <summary>
        ///  The time that will be kept to not immediately place buildings.
        /// </summary>
        private long hoverTime;

        /// <summary>
        /// True if a building is being moved, else false
        /// </summary>
        private bool translateModification;

        /// <summary>
        /// The game object with mesh that the user has chosen that will be placed.
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// The original position of a building that will be moved.
        /// </summary>
        private Vector3 previousPosition;

        /// <summary>
        /// The render components that can be adjusted for the outline
        /// </summary>
        private Renderer[] colorRenders;

        /// <summary>
        /// The current outline shade
        /// </summary>
        private Shader currentShader;

        /// <summary>
        /// Red outline shade
        /// </summary>
        private Shader redOutline = Shader.Find("Outlined/Diffuse_R");

        /// <summary>
        /// Yellow outline shade
        /// </summary>
        private Shader yellowOutline = Shader.Find("Outlined/Diffuse_Y");

        /// <summary>
        /// Red outline shade
        /// </summary>
        private Shader greenOutline = Shader.Find("Outlined/Diffuse_G");

        /// <summary>
        /// The default shading, no outline
        /// </summary>
        private Shader defaultShader = Shader.Find("Standard");

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class. The object will be scaled to 1 meter big.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        /// <param name="gameObject">The game object that has to be placed or moved</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location, GameObject gameObject) : base(stateContext)
        {
            this.translateModification = gameObject.GetComponent<MetaBody>() != null;
            if (this.translateModification)
            {
                this.StateContext.Buttons.InfoText.SetActive(true);
                this.StateContext.Buttons.InfoText.GetComponentInChildren<Text>().text = "Move";
                this.previousPosition = gameObject.transform.position;
                UnityEngine.Object.Destroy(gameObject.GetComponent<MetaBody>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<GroundPlane>());
            }
            else
            {
                this.SetScale(gameObject);
            }

            this.hoverTime = StopwatchSingleton.Time;
            this.gameObject = gameObject;
            this.StateContext.Buttons.BackButton.SetActive(true);
            this.gameObject.transform.position = location;
            this.colorRenders = gameObject.transform.GetComponentsInChildren<Renderer>();
            this.ChangeOutlineRender(this.greenOutline);
        }

        /// <summary>
        /// Shows the position of the to be placed object and places it after having hovered for 3 seconds.
        /// </summary>
        /// <param name="position">position of the to be placed building</param>
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            if ((position - this.gameObject.transform.position).magnitude > (position.magnitude / 10f))
            {
                this.ChangeOutlineRender(this.yellowOutline);
                this.hoverTime = time;
            }
            else
            {
                this.ChangeOutlineRender(this.greenOutline);
            }

            this.gameObject.transform.position = position;
            if (time - this.hoverTime > 2500)
            {
                if (time - this.hoverTime < 2750)
                {
                    this.PlaceBuilding();
                } 
                else
                {
                    this.hoverTime = time;
                }
            }
        }

        /// <summary>
        /// If colliding with an other object, the indicator turns red and you cannot place a building.
        /// </summary>
        /// <param name="gameObject">the game object that was pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
            this.hoverTime = StopwatchSingleton.Time;
            this.ChangeOutlineRender(this.redOutline);
        }

        /// <summary>
        /// Go back to the neutral or modify state based on the state that called it.
        /// </summary>
        public override void OnBackButton()
        {
            if (this.translateModification)
            {
                this.gameObject.transform.position = this.previousPosition;
                this.PlaceBuilding();
            }
            else
            {
                UnityEngine.Object.Destroy(this.gameObject);
                this.StateContext.SetState(new NeutralState(this.StateContext));
            }
        }

        /// <summary>
        /// Place a new building at the pointed location.
        /// </summary>
        private void PlaceBuilding()
        {
            this.gameObject.AddComponent<GroundPlane>();
            this.gameObject.AddComponent<MetaBody>();
            this.ChangeOutlineRender(this.defaultShader);
            this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
        }

        /// <summary>
        /// Change the outline color
        /// </summary>
        /// <param name="shader">New outline shade</param>
        private void ChangeOutlineRender(Shader shader)
        {
            if (shader != this.currentShader)
            {
                for (int i = 0; i < this.colorRenders.Length; i++)
                {
                    this.currentShader = shader;
                    this.colorRenders[i].material.shader = shader;
                }
            }
        }

        /// <summary>
        /// Set the scale of the game object to have a width or height of max 1 meter
        /// </summary>
        /// <param name="gameObject">the game object that will be placed</param>
        private void SetScale(GameObject gameObject)
        {
            Bounds totalBounds = gameObject.GetComponentInChildren<Renderer>().bounds;
            Renderer[] colliders = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < colliders.Length; i++)
            {
                totalBounds.Encapsulate(colliders[i].bounds);
            }

            Vector3 bound = new Vector3(totalBounds.size.x, totalBounds.size.y, totalBounds.size.z);
            float boundScale = 1 / Mathf.Max(bound.z, bound.x);
            gameObject.transform.localScale = new Vector3(boundScale, boundScale, boundScale);
        }
    }
}
