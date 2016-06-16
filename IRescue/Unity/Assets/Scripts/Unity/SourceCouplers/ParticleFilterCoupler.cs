// <copyright file="ParticleFilterCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.Core.DataTypes;
using IRescue.UserLocalisation;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;
using IRescue.UserLocalisation.Particle.Algos.Smoothers;
using IRescue.UserLocalisation.PosePrediction;
using IRescue.UserLocalisation.Sensors;

using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

/// <summary>
///  This class couples sources to a localizer.
/// </summary>
public class ParticleFilterCoupler : AbstractLocalizerCoupler
{
    /// <summary>
    /// The used filter.
    /// </summary>
    private ParticleFilter localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleFilterCoupler"/> class
    /// </summary>
    /// <param name="fieldSize">The preferred game field size</param>
    public ParticleFilterCoupler(FieldSize fieldSize)
    {
        int particleamount = 300;
        IParticleGenerator prtclgen = new RandomParticleGenerator(new ContinuousUniform());
        INoiseGenerator noisegen = new RandomNoiseGenerator(new ContinuousUniform());
        IResampler resampler = new MultinomialResampler();
        ISmoother smoother = new MovingAverageSmoother(1000);
        this.localizer = new ParticleFilter(particleamount, 0.1f, fieldSize, prtclgen, resampler, noisegen, smoother);
    }

    /// <summary>
    /// Return the localizer filter
    /// </summary>
    /// <returns>the localizer</returns>
    public override IUserLocalizer GetLocalizer()
    {
        return this.localizer;
    }

    /// <summary>
    /// Register a acceleration source 
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected override bool RegisterAccelerationReceiver(IAccelerationSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a displacement source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected override bool RegisterDisplacementReceiver(IDisplacementSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a orientation source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected override bool RegisterOrientationReceiver(IOrientationSource source)
    {
        if (source != null)
        {
            this.localizer.AddOrientationSource(source);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a position source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected override bool RegisterPositionReceiver(IPositionSource source)
    {
        if (source != null)
        {
            this.localizer.AddPositionSource(source);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a velocity source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected override bool RegisterVelocityReceiver(IVelocitySource source)
    {
        return false;
    }
}
