﻿// <copyright file="EulerAnglesConversionTests.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Utils;

using IRescue.Core.DataTypes;

using MathNet.Numerics;

using NUnit.Framework;

using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Tests for converting euler angles.
/// </summary>
public class EulerAnglesConversionTests
{
    /// <summary>
    /// Test for the initialize method
    /// </summary>
    [Test]
    public void UnityToIRescue()
    {
        Vector3 xyzd = new Vector3(90, -45, -45);
        Vector3 zxyd = new Vector3(45, -90, -45);
        Vector3 actual = EulerAnglesConversion.ZXYtoXYZ(zxyd);
        Assert.AreEqual(xyzd.ToString(), actual.ToString());

        xyzd = new Vector3(90, 90, -90);
        zxyd = new Vector3(0, -90, 180);
        actual = EulerAnglesConversion.ZXYtoXYZ(zxyd);

        Assert.AreEqual(xyzd.ToString(), actual.ToString());

        xyzd = new Vector3(0, 0, 0);
        zxyd = new Vector3(0, 0, 0);
        actual = EulerAnglesConversion.ZXYtoXYZ(zxyd);
        Assert.AreEqual(xyzd.ToString(), actual.ToString());
    }

    [Test]
    public void IRescueToUnity()
    {
        Vector3 xyzd = new Vector3(90, -45, -45);
        Vector3 zxyd = new Vector3(45, -90, -45);
        Assert.AreEqual(Quaternion.Euler(zxyd).eulerAngles.ToString(), EulerAnglesConversion.XYZtoQuaternion(xyzd).eulerAngles.ToString());

        xyzd = new Vector3(180, 180, 180);
        zxyd = new Vector3(0, 0, 0);
        Assert.AreEqual(Quaternion.Euler(zxyd).eulerAngles.ToString(), EulerAnglesConversion.XYZtoQuaternion(xyzd).eulerAngles.ToString());

        xyzd = new Vector3(-90, 90, 90);
        zxyd = new Vector3(0, -90, 180);
        Assert.AreEqual(Quaternion.Euler(zxyd).eulerAngles.ToString(), EulerAnglesConversion.XYZtoQuaternion(xyzd).eulerAngles.ToString());
    }
}
