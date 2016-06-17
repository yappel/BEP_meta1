// <copyright file="ObjectPlacementState.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.ObjectPlacing.States
{
    using IRescue.Core.Utils;
    using Meta;
    using UnityEngine;

    /// <summary>
    /// State when placing a building by pointing.
    /// </summary>
    public class ObjectPlacementState : AbstractState
    {
        /// <summary>
        /// Time in milliseconds required to point steadily to place the building
        /// </summary>
        private const int TimeToPlace = 2000;

        /// <summary>
        /// The preferred size of a created building in meters
        /// </summary>
        private const float PreferredInitSize = 0.5f;

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
        private MeshRenderer[] colorRenders;

        /// <summary>
        /// The current outline shade
        /// </summary>
        private Color currentColor;

        /// <summary>
        /// Red outline shade
        /// </summary>
        private Shader greenOutline = Shader.Find("Outlined/Diffuse_G");

        /// <summary>
        /// The default shading, no outline
        /// </summary>
        private Shader defaultShader = Shader.Find("Standard");

        /// <summary>
        /// Boolean if the user has pointed at the current iteration
        /// </summary>
        private bool hasPointed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class. The object will be scaled to 1 meter big.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        /// <param name="gameObjectPath">Path to the wanted object to place</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location, string gameObjectPath) 
            : this(stateContext, location, CreateObject(gameObjectPath))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPlacementState"/> class. The object will be scaled to 1 meter big.
        /// </summary>
        /// <param name="stateContext">State context</param>
        /// <param name="location">First indicated position of the placement</param>
        /// <param name="gameObject">The game object that has to be placed or moved</param>
        public ObjectPlacementState(StateContext stateContext, Vector3 location, GameObject gameObject) : base(stateContext)
        {
            this.translateModification = gameObject.GetComponent<BuildingPlane>() != null;
            if (this.translateModification)
            {
                this.InitTextPane("InfoText", "Move");
                this.previousPosition = gameObject.transform.localPosition;
                UnityEngine.Object.Destroy(gameObject.GetComponent<BuildingPlane>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<MetaBody>());
            }

            this.hoverTime = StopwatchSingleton.Time;
            this.gameObject = gameObject;
            this.InitButton("BackButton", () => this.OnBackButton());
            this.gameObject.transform.localPosition = location;
            this.colorRenders = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
            this.ChangeOutlineRender(this.greenOutline);
        }

        /// <summary>
        /// Shows the position of the to be placed object and places it after having hovered for 3 seconds.
        /// </summary>
        /// <param name="position">position of the to be placed building</param>
        public override void OnPoint(Vector3 position)
        {
            long time = StopwatchSingleton.Time;
            this.hasPointed = true;
            if ((position - this.gameObject.transform.localPosition).magnitude > (position.magnitude / 20f))
            {
                this.ChangeOutlineRender(Color.yellow);
                this.hoverTime = time;
            }
            else
            {
                this.ChangeOutlineRender(Color.green);
            }

            this.gameObject.transform.localPosition = position;
            if (time - this.hoverTime > TimeToPlace + 250)
            {
                this.hoverTime = time;
            }
            else if (time - this.hoverTime > TimeToPlace)
            {
                this.PlaceBuilding();
            }
        }

        /// <summary>
        /// If colliding with an other object, the indicator turns red and you cannot place a building.
        /// </summary>
        /// <param name="gameObject">the game object that was pointed at</param>
        public override void OnPoint(GameObject gameObject)
        {
            this.hoverTime = StopwatchSingleton.Time;
            this.ChangeOutlineRender(Color.red);
        }

        /// <summary>
        /// Set the outline red if the user is not pointing.
        /// </summary>
        public override void RunLateUpdate()
        {
            if (!this.hasPointed)
            {
                this.ChangeOutlineRender(Color.red);
            }

            this.hasPointed = false;
        }

        /// <summary>
        /// Go back to the neutral or modify state based on the state that called it.
        /// </summary>
        public void OnBackButton()
        {
            if (this.CanSwitchState())
            {
                if (this.translateModification)
                {
                    this.gameObject.transform.localPosition = this.previousPosition;
                    this.PlaceBuilding();
                }
                else
                {
                    UnityEngine.Object.Destroy(this.gameObject);
                    this.StateContext.SetState(new NeutralState(this.StateContext));
                }
            }
        }

        /// <summary>
        /// Initialize a game objects
        /// </summary>
        /// <param name="gameObjectPath">the path to the object</param>
        /// <returns>The initialized game object</returns>
        private static GameObject CreateObject(string gameObjectPath)
        {
            GameObject newObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(gameObjectPath));
            newObject.gameObject.transform.parent = GameObject.Find("GroundPlane").transform;
            SetScale(newObject);
            newObject.transform.localRotation = Quaternion.identity;
            return newObject;
        }

        /// <summary>
        /// Set the scale of the game object to have a width or height of max 1 meter
        /// </summary>
        /// <param name="gameObject">the game object that will be placed</param>
        private static void SetScale(GameObject gameObject)
        {
            Bounds totalBounds = gameObject.GetComponentInChildren<Renderer>().bounds;
            Renderer[] colliders = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < colliders.Length; i++)
            {
                totalBounds.Encapsulate(colliders[i].bounds);
            }

            Vector3 bound = new Vector3(totalBounds.size.x, totalBounds.size.y, totalBounds.size.z);
            Vector3 localFactor = gameObject.transform.parent.localScale;
            float boundScale = PreferredInitSize / Mathf.Max(bound.z, bound.x);
            gameObject.transform.localScale = new Vector3(boundScale / localFactor.x, boundScale / localFactor.y, boundScale / localFactor.z);
        }

        /// <summary>
        /// Place a new building at the pointed location.
        /// </summary>
        private void PlaceBuilding()
        {
            this.gameObject.AddComponent<BuildingPlane>();
            MetaBody mb = this.gameObject.AddComponent<MetaBody>();
            mb.maxScaleRatio = 100;
            this.ChangeOutlineRender(this.defaultShader);
            this.StateContext.SetState(new ModifyState(this.StateContext, this.gameObject));
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

        /// <summary>
        /// Change the outline color
        /// </summary>
        /// <param name="color">New outline color</param>
        private void ChangeOutlineRender(Color color)
        {
            if (color != this.currentColor)
            {
                for (int i = 0; i < this.colorRenders.Length; i++)
                {
                    this.currentColor = color;
                    this.colorRenders[i].material.SetColor("_OutlineColor", color);
                }
            }
        }
    }
}
