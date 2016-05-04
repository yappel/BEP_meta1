// <copyright file="UserController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Meta;
using UnityEngine;

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]

/// <summary>
///  UserController keeps track of the user and its behavior.
/// </summary>
public class UserController : MonoBehaviour
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
    void Start()
    {
        this.markerSensor = new MarkerSensor();
        this.markerDetector = MarkerDetector.Instance;
        this.markerTransform = new GameObject().transform;
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
            IRVector3 position = new IRVector3(this.markerTransform.position.x, this.markerTransform.position.y, this.markerTransform.position.z);
            IRVector3 rotation = new IRVector3(this.markerTransform.rotation.x, this.markerTransform.rotation.y, this.markerTransform.rotation.z);
            visibleMarkerTransforms.Add(markerId, new IRVectorTransform(position, rotation));

            //// Debug.Log("Marker " + markerId + " visible with date position = (" 
            ////   + markerTransform.position.x + "," + markerTransform.position.y + "," + markerTransform.position.z + "), rotation = ("
            //// + markerTransform.rotation.x + "," + markerTransform.rotation.y + "," + markerTransform.rotation.z + ")");
        }

        return visibleMarkerTransforms;
    }
}
