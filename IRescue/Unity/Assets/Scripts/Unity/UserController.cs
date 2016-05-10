// <copyright file="UserController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.Core.DataTypes;
using IRescue.Core.Utils;
using IRescue.UserLocalisation;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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
    ///   Method calles on every frame.
    /// </summary>
    public void Update()
    {
        Pose pose = localizer.CalculatePose(StopwatchSingleton.Time);
        this.transform.position = this.TransformVector(pose.Position);
        this.transform.eulerAngles = this.TransformVector(pose.Orientation);
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
