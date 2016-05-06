// <copyright file="MarkerSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Inputsensors;
using Assets.Scripts.Unity.SensorControllers;
using Meta;
using UnityEngine;

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]

/// <summary>
///   This class keeps track of visible markers and the probable user location based on that.
/// </summary>
public class MarkerSensorController : AbstractSensorController
{
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
    ///   Method called when creating a UserController.
    /// </summary>
    public override void Init()
    {
        this.markerSensor = new MarkerSensor();
        this.markerDetector = MarkerDetector.Instance;
        this.markerTransform = new GameObject().transform;
    }

    /// <summary>
    ///   Return the motion source.
    /// </summary>
    /// <returns>the ILocationSource</returns>
    public new ILocationSource GetLocationSource()
    {
        return this.markerSensor;
    }

    /// <summary>
    ///   Return the motion source.
    /// </summary>
    /// <returns>The IRotationSource</returns>
    public new IRotationSource GetRotationSource()
    {
        return this.markerSensor;
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    void Update()
    {
        this.markerSensor.UpdateLocations(this.GetVisibleMarkers());
    }

    /// <summary>
    ///   Get all the transforms of the visible markers.
    /// </summary>
    /// <returns>Hash table with the marker id as the key and an IRVectorTransform as the value.</returns>
    private Dictionary<int, IRVectorTransform> GetVisibleMarkers()
    {
        List<int> visibleMarkers = this.markerDetector.updatedMarkerTransforms;
        Dictionary<int, IRVectorTransform> visibleMarkerTransforms = new Dictionary<int, IRVectorTransform>();

        for (int i = 0; i < visibleMarkers.Count; i++)
        {
            int markerId = visibleMarkers[i];
            this.markerDetector.SetMarkerTransform(markerId, ref this.markerTransform);
            if (GameObject.Find("MarkerIndicators/MarkerIndicator" + markerId) != null)
            {
                var marker = GameObject.Find("MarkerIndicators/MarkerIndicator" + markerId).transform.eulerAngles;
                IRVector3 position = new IRVector3(this.markerTransform.position.x, this.markerTransform.position.y, this.markerTransform.position.z);
                IRVector3 rotation = new IRVector3(marker.x, marker.y, marker.z);
                visibleMarkerTransforms.Add(markerId, new IRVectorTransform(position, rotation));
            }
        }

        return visibleMarkerTransforms;
    }
}
