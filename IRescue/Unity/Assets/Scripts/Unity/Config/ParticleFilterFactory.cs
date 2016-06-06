namespace Assets.Scripts.Unity.Config
{
    using IniParser.Exceptions;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;

    using MathNet.Numerics.Distributions;

    public class ParticleFilterFactory
    {
        public static ParticleFilter Create(
            int particleAmount,
            float resampleNoiseSize,
            FieldSize fieldSize,
            string particleGeneratorString,
            string resamplerString,
            string noiseGeneratorString,
            string smootherString)
        {
            IParticleGenerator particleGenerator = CreateParticleGenerator(particleGeneratorString);
            IResampler resampler = CreateResampler(resamplerString);
            INoiseGenerator noiseGenerator = CreateNoiseGenerator(noiseGeneratorString);
            ISmoother smoother = CreateSmoother(smootherString);
            return new ParticleFilter(particleAmount, resampleNoiseSize, fieldSize, particleGenerator, resampler, noiseGenerator, smoother);
        }

        /// <summary>Creates a <see cref="ISmoother"/> based on a string value</summary>
        /// <param name="smootherString">The string value that decides the kind of <see cref="ISmoother"/> created.</param>
        /// <returns>The <see cref="ISmoother"/>.</returns>
        /// <exception cref="ParsingException">Thrown when there is no kind of <see cref="ISmoother"/> associated with the string value.</exception>
        private static ISmoother CreateSmoother(string smootherString)
        {
            switch (smootherString)
            {
                case "movingaverage2sec":
                    return new MovingAverageSmoother(2000);
                case "movingaverage1sec":
                    return new MovingAverageSmoother(1000);
                case "movingaverage0_5sec":
                    return new MovingAverageSmoother(500);
                default:
                    throw new ParsingException(string.Format("There is no kind of smoothing algorithm associated with the value {0}", smootherString));

            }
        }

        /// <summary>Creates a <see cref="INoiseGenerator"/> based on a string value</summary>
        /// <param name="noiseGeneratorString">The string value that decides the kind of <see cref="INoiseGenerator"/> created.</param>
        /// <returns>The <see cref="INoiseGenerator"/>.</returns>
        /// <exception cref="ParsingException">Thrown when there is no kind of <see cref="INoiseGenerator"/> associated with the string value.</exception>
        private static INoiseGenerator CreateNoiseGenerator(string noiseGeneratorString)
        {
            switch (noiseGeneratorString)
            {
                case "randomuniform":
                    return new RandomNoiseGenerator(new ContinuousUniform());
                default:
                    throw new ParsingException(string.Format("There is no kind of noise generator associated with the value {0}", noiseGeneratorString));
            }
        }

        /// <summary>Creates a <see cref="IResampler"/> based on a string value</summary>
        /// <param name="resamplerString">The string value that decides the kind of <see cref="IResampler"/> created.</param>
        /// <returns>The <see cref="IResampler"/>.</returns>
        /// <exception cref="ParsingException">Thrown when there is no kind of <see cref="IResampler"/> associated with the string value.</exception>
        private static IResampler CreateResampler(string resamplerString)
        {

            switch (resamplerString)
            {
                case "multinomial":
                    return new MultinomialResampler();
                default:
                    throw new ParsingException(string.Format("There is no kind of resampling algorithm associated with the value {0}", resamplerString));
            }
        }

        /// <summary>Creates a <see cref="IParticleGenerator"/> based on a string value</summary>
        /// <param name="particleGenerator">The string value that decides the kind of <see cref="IParticleGenerator"/> created.</param>
        /// <returns>The <see cref="IParticleGenerator"/>.</returns>
        /// <exception cref="ParsingException">Thrown when there is no kind of <see cref="IParticleGenerator"/> associated with the string value.</exception>
        private static IParticleGenerator CreateParticleGenerator(string particleGenerator)
        {
            switch (particleGenerator)
            {
                case "randomuniform":
                    return new RandomParticleGenerator(new ContinuousUniform());
                default:
                    throw new ParsingException(string.Format("There is no kind of particle generator associated with the value {0}", particleGenerator));
            }
        }
    }
}
