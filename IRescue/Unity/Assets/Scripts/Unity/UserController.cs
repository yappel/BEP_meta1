// <copyright file="UserController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.UserLocalisation.Particle;
using UnityEngine;

/// <summary>
///  UserController keeps track of the user and its behavior.
/// </summary>
public class UserController : MonoBehaviour
{
    /// <summary>
    /// The used localizer.
    /// </summary>
    private MonteCarloLocalizer localizer;

    /// <summary>
    /// Initializes the User Controller. 
    /// </summary>
    /// <param name="localizer">The <see cref="MonteCarloLocalizer"/> used for the localization.</param>
    public void Init(MonteCarloLocalizer localizer)
    {
        this.localizer = localizer;
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    public void Update()
    {
        this.transform.position = this.TransformVector(this.localizer.GetPosition());
        this.transform.eulerAngles = this.TransformVector(this.localizer.GetRotation());
    }

    /// <summary>
    ///   Transform a IRescue.Core.DataTypes.Vector3 to a Unity.Vector3. 
    /// </summary>
    /// <param name="vector">The IRescue.Core.DataTypes.Vector3</param>
    /// <returns>Unity.Vector3 instance</returns>
    private Vector3 TransformVector(IRescue.Core.DataTypes.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}
