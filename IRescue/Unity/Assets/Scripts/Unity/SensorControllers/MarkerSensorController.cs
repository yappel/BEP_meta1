﻿// <copyright file="MarkerSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Assets.Scripts.Unity.SensorControllers;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisation.Sensors.Marker;
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
    private float positionStd = 0.0f;

    /// <summary>
    /// standard deviation for orientation
    /// </summary>
    private float orientationStd = 2;

    /// <summary>
    ///   Method called when creating a UserController.
    /// </summary>
    public override void Init()
    {
        this.markerSensor = new MarkerSensor(this.positionStd, Path);
        this.markerDetector = MarkerDetector.Instance;
        this.markerTransform = new GameObject().transform;
    }

    /// <summary>
    ///   Return the Orientation source.
    /// </summary>
    /// <returns>The IOrientationSource</returns>
    public new IOrientationSource GetOrientationSource()
    {
        return this.markerSensor;
    }

    /// <summary>
    ///   Return the position source.
    /// </summary>
    /// <returns>the IPositionSource</returns>
    public new IPositionSource GetPositionSource()
    {
        return this.markerSensor;
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    public void Update()
    {
        this.markerSensor.UpdateLocations(this.GetVisibleMarkers());
    }

    /// <summary>
    ///   Get all the transforms of the visible markers.
    /// </summary>
    /// <returns>Hash table with the marker id as the key and an IRVectorTransform as the value.</returns>
    private Dictionary<int, Pose> GetVisibleMarkers()
    {
        List<int> visibleMarkers = this.markerDetector.updatedMarkerTransforms;
        Dictionary<int, Pose> visibleMarkerTransforms = new Dictionary<int, Pose>();

        for (int i = 0; i < visibleMarkers.Count; i++)
        {
            int markerId = visibleMarkers[i];
            this.markerDetector.SetMarkerTransform(markerId, ref this.markerTransform);
            if (GameObject.Find("MarkerIndicators/MarkerIndicator" + markerId) != null)
            {
                var marker = GameObject.Find("MarkerIndicators/MarkerIndicator" + markerId).transform.eulerAngles;
                Vector3 position = new Vector3(this.markerTransform.position.x, this.markerTransform.position.y, this.markerTransform.position.z);
                Vector3 rotation = new Vector3(marker.x, marker.y, marker.z);
                visibleMarkerTransforms.Add(markerId, new Pose(position, rotation));
            }
        }

        return visibleMarkerTransforms;
    }
}
