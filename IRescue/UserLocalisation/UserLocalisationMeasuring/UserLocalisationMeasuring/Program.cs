using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;
using IRescue.UserLocalisation.PosePrediction;
using IRescue.UserLocalisationMeasuring.DataGeneration;
using IRescue.UserLocalisationMeasuring.DataProcessing;
using MathNet.Numerics.Random;

namespace IRescue.UserLocalisationMeasuring
{
    public static class Program
    {
        private const int PARTICLEAMOUNT = 30;
        private const int NOISEAMOUNT = 0;
        private const int CDFRANGE = 0;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //CalcBestParticleSettings();

        }

        private static void PlotParticleAmount()
        {
            for (int count = 0; count < 1000; count = count + 10)
            {

            }
        }

        private static void PloteNoiseAmount()
        {

        }

        private static void PlotCDFRange()
        {

        }

        private static void PlotResampleAlgos()
        {

        }

        private static void PlotPredictionAlgos()
        {

        }

        public static void CalcBestParticleSettings()
        {
            //TODO create scenarios
            FieldSize fieldSize = new FieldSize() { Xmax = 2, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 2, Zmin = 0 };
            OrientationScenario oriscen1 = null;
            OrientationScenario oriscen2 = null;
            OrientationScenario oriscen3 = null;
            PositionScenario posscen1 = null;
            PositionScenario posscen2 = null;
            PositionScenario posscen3 = null;
            IParticleGenerator particlegen = new RandomParticleGenerator(new SystemRandomSource());
            INoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
            IResampler resampler = new MultinomialResampler();
            IPosePredictor posepredictor = new LinearPosePredicter();
            double[] maxima = new double[] { 1, 1, 1, 1, 1, 1 };
            double bestprecision1 = 0;
            double bestprecision2 = 0;
            double bestprecision3 = 0;
            Console.WriteLine("Scen" + "\t" + "particle" + "\t" + "noise" + "\t" + "cdfmargin");
            for (int particles = 0; particles < 1000; particles += 100)
            {
                for (float noise = 0; noise < 1; noise += 0.1f)
                {
                    for (double cdfmargin = 0; cdfmargin < 0.5; cdfmargin += 0.05)
                    {
                        ParticleFilter filter1 = new ParticleFilter(fieldSize, particles, cdfmargin, noise, particlegen,
                            posepredictor, noisegen, resampler);
                        filter1.AddOrientationSource(oriscen1);
                        filter1.AddPositionSource(posscen1);
                        ParticleFilter filter2 = new ParticleFilter(fieldSize, particles, cdfmargin, noise, particlegen,
                            posepredictor, noisegen, resampler);
                        filter2.AddOrientationSource(oriscen2);
                        filter2.AddPositionSource(posscen2);
                        ParticleFilter filter3 = new ParticleFilter(fieldSize, particles, cdfmargin, noise, particlegen,
                            posepredictor, noisegen, resampler);
                        filter3.AddOrientationSource(oriscen3);
                        filter3.AddPositionSource(posscen3);
                        LocalizerAnalyser locanal1 = new LocalizerAnalyser(10, 10, filter1, posscen1, oriscen1);
                        if (bestprecision1 < locanal1.Precision)
                        {
                            bestprecision1 = locanal1.Precision;
                            Console.WriteLine("1" + "\t" + particles + "\t" + noise + "\t" + cdfmargin);
                        }
                        LocalizerAnalyser locanal2 = new LocalizerAnalyser(10, 10, filter2, posscen2, oriscen2);
                        if (bestprecision2 < locanal2.Precision)
                        {
                            bestprecision1 = locanal1.Precision;
                            Console.WriteLine("2" + "\t" + particles + "\t" + noise + "\t" + cdfmargin);
                        }
                        LocalizerAnalyser locanal3 = new LocalizerAnalyser(10, 10, filter3, posscen3, oriscen3);
                        if (bestprecision3 < locanal3.Precision)
                        {
                            bestprecision1 = locanal1.Precision;
                            Console.WriteLine("3" + "\t" + particles + "\t" + noise + "\t" + cdfmargin);
                        }
                    }
                }
            }
        }

        private static void PlotParticle()
        {

        }

        private static void PlotKalman()
        {

        }

        private static void PlotSimple()
        {

        }
    }
}
