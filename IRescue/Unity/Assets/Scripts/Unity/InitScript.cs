// <copyright file="InitScript.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Assets.Scripts.Unity.Config;
    using Assets.Scripts.Unity.WaterTracking;

    using Enums;
    using IRescue.UserLocalisation;
    using ObjectPlacing;
    using SensorControllers;
    using SourceCouplers;
    using UnityEngine;

    /// <summary>
    ///  This script initialized the entire setup. This is the only script that should be added to a GameObject in the Unity editor.
    /// </summary>
    public class InitScript : MonoBehaviour
    {
        /// <summary>
        /// Size of the used markers in meters
        /// </summary>
        private float markerSize = 0.23f;

        private GeneralConfigs generalConfigs;

        private MarkerConfigs markerConfigs;

        private AbstractLocalizerCoupler localizerCoupler;

        /// <summary>
        ///  Called when the game starts.
        /// </summary>
        public void Start()
        {
            // Start in 2d mode
            Meta.MetaCameraMode.monocular = true;
            Meta.MarkerDetector.Instance.SetMarkerSize(this.markerSize);
            this.markerConfigs = new MarkerConfigs();
            this.generalConfigs = new GeneralConfigs(this.markerConfigs);
            this.localizerCoupler = LocalizerCouplerFactory.Get(this.generalConfigs.UserLocalizer);

            //Init sensors
            this.InitSensorControllers();
            //Init World controller
            this.InitWordControllers();
            //Init user input controller(s)
            this.InitInputHandlers();
        }

        /// <summary>
        /// Initializes the controllers that handle the data retrieved from the sensors.
        /// </summary>
        private void InitSensorControllers()
        {
            if (!this.generalConfigs.IgnoreIMUData)
            {
                ImuSensorController component = this.gameObject.AddComponent<ImuSensorController>();
                component.Init(this.generalConfigs.IMUSource);
                this.localizerCoupler.RegisterSource(component);
            }

            if (!this.generalConfigs.IgnoreMarkers)
            {
                MarkerSensorController component = this.gameObject.AddComponent<MarkerSensorController>();
                component.Init(this.generalConfigs.MarkerSensor);
                this.InitMarker();
                this.localizerCoupler.RegisterSource(component);
            }
        }

        /// <summary>
        /// Initializes the controllers that controll the objects in the virtual world.
        /// </summary>
        private void InitWordControllers()
        {
            //TODO change to worldbox
            this.InitPlanes(200, 200);
            this.gameObject.AddComponent<UserController>().Init(this.generalConfigs.UserLocalizer);
        }

        /// <summary>
        /// Initializes the scripts that handle user input.
        /// </summary>
        private void InitInputHandlers()
        {
            this.gameObject.AddComponent<GestureEventController>().Init();
        }


        /// <summary>
        ///  Add the controllers to the GameObject.
        /// </summary>
        private void AddControllers()
        {

            IEnumerable<AbstractSensorController> sensorControllers = this.GetAbstractControllers();
            foreach (AbstractSensorController controller in sensorControllers)
            {
                this.gameObject.AddComponent(controller.GetType());
            }
        }

        /// <summary>
        ///  Get all the Scripts that inherit <see cref="AbstractSensorController"/>.
        /// </summary>
        /// <returns>List of the controller class types that inherit <see cref="AbstractSensorController"/></returns>
        private IEnumerable<AbstractSensorController> GetAbstractControllers()
        {
            return typeof(AbstractSensorController)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(AbstractSensorController)) && !t.IsAbstract)
                .Select(t => (AbstractSensorController)Activator.CreateInstance(t));
        }

        /// <summary>
        /// Create a cube with marker target behavior so that markers are tracked
        /// </summary>
        private void InitMarker()
        {
            GameObject cube = new GameObject();
            cube.AddComponent<Meta.MetaBody>().markerTarget = true;
        }

        /// <summary>
        /// Create the ground and water plane
        /// </summary>
        /// <param name="width">width of the plane, or x</param>
        /// <param name="depth">depth of the plane, or z</param>
        private void InitPlanes(float width, float depth)
        {
            GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundPlane.AddComponent<GroundPlane>().Init(width, depth);
            GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            waterPlane.AddComponent<WaterLevelController>().Init(width, depth);
        }
    }
}
