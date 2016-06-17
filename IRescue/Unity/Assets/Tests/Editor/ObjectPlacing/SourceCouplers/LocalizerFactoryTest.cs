// <copyright file="LocalizerFactoryTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Enums;
using Assets.Scripts.Unity.SourceCouplers;
using IRescue.Core.DataTypes;
using NUnit.Framework;

/// <summary>
/// Tests for <see cref="LocalizerFactory"/>
/// </summary>
public class LocalizerFactoryTest
{
    /// <summary>
    /// Test for the particle get
    /// </summary>
    [Test]
    public void ParticleTest()
    {
        FieldSize fieldSize = new FieldSize() { Xmax = 4, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
        Assert.True(LocalizerFactory.Get(Filters.Particle, fieldSize) is ParticleFilterCoupler);
    }
}
