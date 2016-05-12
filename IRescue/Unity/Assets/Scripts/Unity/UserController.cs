// <copyright file="UserController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.UserLocalisation;
using UnityEngine;

/// <summary>
///  UserController keeps track of the user and its behavior.
/// </summary>
public class UserController : MonoBehaviour
{
    /// <summary>
    /// The used localizer.
    /// </summary>
    private AbstractUserLocalizer localizer;

    /// <summary>
    /// Initializes the User Controller. 
    /// </summary>
    /// <param name="localizer">The <see cref="AbstractUserLocalizer"/> used for the localization.</param>
    public void Init(AbstractUserLocalizer localizer)
    {
        this.localizer = localizer;
    }

    /// <summary>
    /// Last code that gets executed to update the rotation and position correctly.
    /// </summary>
    public void LateUpdate()
    {
        this.transform.position = this.TransformVector(this.localizer.GetPosition());
        this.transform.GetChild(0).eulerAngles = this.TransformVector(this.localizer.GetRotation());
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
