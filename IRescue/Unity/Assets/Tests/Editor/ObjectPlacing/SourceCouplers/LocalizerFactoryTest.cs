// <copyright file="LocalizerFactoryTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Enums;
using Assets.Scripts.Unity.SourceCouplers;
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
        Assert.True(LocalizerFactory.Get(Filters.Particle) is ParticleFilterCoupler);
    }
}
