// <copyright file="ParticleFilterCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.Core.DataTypes;
using IRescue.UserLocalisation;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;
using IRescue.UserLocalisation.PosePrediction;
using IRescue.UserLocalisation.Sensors;
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
    public ParticleFilterCoupler()
    {
        FieldSize fieldSize = new FieldSize() { Xmax = 2, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 2, Zmin = 0 };
        int particleamount = 30;
        RandomParticleGenerator prtclgen = new RandomParticleGenerator(new SystemRandomSource());
        LinearPosePredicter posePredictor = new LinearPosePredicter();
        RandomNoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
        MultinomialResampler resampler = new MultinomialResampler();
        this.localizer = new ParticleFilter(fieldSize, particleamount, 0.005f, 0.0f, prtclgen, posePredictor, noisegen, resampler);
    }

    /// <summary>
    /// Return the localizer filter
    /// </summary>
    /// <returns>the localizer</returns>
    public override AbstractUserLocalizer GetLocalizer()
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
