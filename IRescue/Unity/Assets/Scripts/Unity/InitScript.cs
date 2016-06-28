// <copyright file="InitScript.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using IRescue.Core.DataTypes;
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
        /// Type of the filter that is going to be used
        /// </summary>
        private Filters usedFilter = Filters.Particle;

        /// <summary>
        /// Size of the used markers in meters
        /// </summary>
        private float markerSize = 0.23f;

        /// <summary>
        ///  Called when the game starts.
        /// </summary>
        public void Start()
        {
            Meta.MetaCameraMode.monocular = true;
            Meta.MarkerDetector.Instance.SetMarkerSize(this.markerSize);
            this.AddControllers();
            FieldSize fieldSize = new FieldSize() { Xmax = 15, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 20, Zmin = 0 };
            AbstractLocalizerCoupler coupler = LocalizerFactory.Get(this.usedFilter, fieldSize);
            this.InitControllers(coupler);
            this.InitMarker();
            this.InitWorldBox(coupler.GetLocalizer(), fieldSize);
            Meta.MetaUI.Instance.enableGrid = false;
        }

        /// <summary>
        ///  Add the controllers to the GameObject.
        /// </summary>
        private void AddControllers()
        {
            gameObject.AddComponent<GestureEventController>().Init();
            IEnumerable<AbstractSensorController> sensorControllers = this.GetAbstractControllers();
            foreach (AbstractSensorController controller in sensorControllers)
            {
                gameObject.AddComponent(controller.GetType());
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
        ///  Initialize the controllers and register them to a localizer. 
        ///  Disable the controller if its source is not required by the current localizer method.
        /// </summary>
        /// <param name="localizer">The Localizer filter</param>
        private void InitControllers(AbstractLocalizerCoupler localizer)
        {
            AbstractSensorController[] controllers = gameObject.GetComponents<AbstractSensorController>();
            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i].Init();
                controllers[i].enabled = localizer.RegisterSource(controllers[i]);
            }
        }

        /// <summary>
        /// Create a cube with marker target behavior so that markers are tracked
        /// </summary>
        private void InitMarker()
        {
            GameObject cube = new GameObject("MarkerMetaBody");
            cube.AddComponent<Meta.MetaBody>().markerTarget = true;
        }

        /// <summary>
        /// Create the ground and water plane
        /// </summary>
        /// <param name="localizer">Localizer filter used for position tracking</param>
        /// <param name="fieldSize">Size of the field</param>
        private void InitWorldBox(IUserLocalizer localizer, FieldSize fieldSize)
        {
            GameObject worldBox = new GameObject("WorldBox");
            worldBox.AddComponent<WorldBox>().Init(localizer, fieldSize);
        }
    }
}
