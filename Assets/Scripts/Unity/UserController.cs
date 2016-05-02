// <copyright file="UserController.cs" company="Delft Universite of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections;
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
    private PredictionWeightBuffer predictionWeightBuffer;
    
    /// <summary>
    ///   Method called when creating a UserController.
    /// </summary>
    void Start()
    {
        this.predictionWeightBuffer = new PredictionWeightBuffer();
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    void Update()
    {
        IRVectorTransform newLocation = this.predictionWeightBuffer.predictLocation(null);
        transform.position = new Vector3(newLocation.GetPosition().GetX(), newLocation.GetPosition().GetY(), newLocation.GetPosition().GetZ());
        transform.rotation = new Quaternion(newLocation.GetRotation().GetX(), newLocation.GetRotation().GetY(), newLocation.GetRotation().GetZ(), 1);
    }
}
