// <copyright file="MarkerSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Assets.Scripts.Unity.SensorControllers;
using Assets.Scripts.Unity.Utils;

using IRescue.Core.DataTypes;
using IRescue.Core.Distributions;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisation.Sensors.Marker;

using MathNet.Numerics.LinearAlgebra;

using Meta;
using UnityEngine;
using Vector3 = IRescue.Core.DataTypes.Vector3;

/// <summary>
///   This class keeps track of visible markers and the probable user location based on that.
/// </summary>
public class MarkerSensorController : AbstractSensorController
{
    /// <summary>
    /// Path to the save file
    /// </summary>
    private const string Path = "./Assets/Maps/MarkerMap01.xml";

    /// <summary>
    ///   Keeps track of the user location and calculates the new one on update.
    /// </summary>
    private MarkerSensor markerSensor;

    /// <summary>
    ///   The Singleton instance of MarkerDetector.
    /// </summary>
    private MarkerDetector markerDetector;

    /// <summary>
    ///   A GameObject of which the transform will be used for reference purpose.
    /// </summary>
    private Transform markerTransform;

    /// <summary>
    ///   The standard deviation of the meta sensor for markers.
    /// </summary>
    private float positionStd = 0.00945f;

    /// <summary>
    /// standard deviation for orientation
    /// </summary>
    private float orientationStd = 0.264f;

    /// <summary>
    ///   Method called when creating a UserController.
    /// </summary>
    public override void Init()
    {
        this.markerSensor = new MarkerSensor(new MarkerLocations(Path), new Normal(this.orientationStd), new Normal(this.positionStd));
        this.markerDetector = MarkerDetector.Instance;
        this.markerTransform = new GameObject().transform;
    }

    /// <summary>
    ///   Return the position source.
    /// </summary>
    /// <returns>the IPositionSource</returns>
    public override IPositionSource GetPositionSource()
    {
        return this.markerSensor;
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    public void Update()
    {
        if (this.ImuInitialized())
        {
            long t = IRescue.Core.Utils.StopwatchSingleton.Time;
            this.markerSensor.UpdateLocations(t, this.GetVisibleMarkers());
        }
    }

    /// <summary>
    ///   Get all the transforms of the visible markers.
    /// </summary>
    /// <returns>Hash table with the marker id as the key and an IRVectorTransform as the value.</returns>
    private Dictionary<int, TransformationMatrix> GetVisibleMarkers()
    {
        List<int> visibleMarkers = this.markerDetector.updatedMarkerTransforms;
        Dictionary<int, TransformationMatrix> visibleMarkerTransforms = new Dictionary<int, TransformationMatrix>();

        for (int i = 0; i < visibleMarkers.Count; i++)
        {
            int markerId = visibleMarkers[i];
            UnityEngine.Vector3 MetaOrientation = EulerAnglesConversion.ZXYtoXYZ(IMULocalizer.Instance.localizerOrientation);
            this.markerDetector.SetMarkerTransform(markerId, ref this.markerTransform);
            ////Remove meta sdk added rotation for horizontal markers
            this.markerTransform.Rotate(UnityEngine.Vector3.right, 90f);
            UnityEngine.Vector3 xyzAngles = EulerAnglesConversion.ZXYtoXYZ(this.markerTransform.eulerAngles);
            TransformationMatrix Tcm = new TransformationMatrix(
                this.markerTransform.position.x,
                this.markerTransform.position.y,
                this.markerTransform.position.z,
                xyzAngles.x,
                xyzAngles.y,
                xyzAngles.z);

            // TODO optimize?
            TransformationMatrix Tcu = new TransformationMatrix(0, 0, 0, MetaOrientation.x, MetaOrientation.y + 180, MetaOrientation.z);
            TransformationMatrix Tum = new TransformationMatrix { [3, 3] = 1 };
            Tcu.Inverse().Multiply(Tcm, Tum);

            visibleMarkerTransforms.Add(markerId, Tum);


            //IRescue.Core.DataTypes.Vector4 relativeMarkerPosition = new IRescue.Core.DataTypes.Vector4 { [3] = 1 };
            //Tum.Multiply(relativeMarkerPosition, relativeMarkerPosition);
            //// TODO fix to create Vector3 by copying from Vector4?
            //Vector3 position = new Vector3(relativeMarkerPosition.X, relativeMarkerPosition.Y, relativeMarkerPosition.Z);

            //Vector3 rotation = new Vector3(
            //    this.markerTransform.eulerAngles.x - MetaOrientation.x,
            //    180 + this.markerTransform.eulerAngles.y - MetaOrientation.y,
            //    this.markerTransform.eulerAngles.z - MetaOrientation.z);
            //visibleMarkerTransforms.Add(markerId, new Pose(position, rotation));
        }

        return visibleMarkerTransforms;
    }
}
