// <copyright file="LocalizerAnalyser.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisationMeasuring.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisationMeasuring.DataGeneration;

    /// <summary>
    /// Analyzes the a localisation filter and calculates metrics.
    /// </summary>
    public class LocalizerAnalyser
    {
        private List<float> orix;

        private List<float> oriy;

        private List<float> oriz;

        private List<float> posx;

        private List<float> posy;

        private List<float> posz;

        public LocalizerAnalyser(
            int cycleamount,
            List<ParticleFilter> filters,
            PositionScenario posscen,
            OrientationScenario oriscen,
            int sceneid,
            double cdfmargin,
            double noise,
            int algos)
        {
            this.posx = new List<float>();
            this.posy = new List<float>();
            this.posz = new List<float>();
            this.orix = new List<float>();
            this.oriy = new List<float>();
            this.oriz = new List<float>();
            StringBuilder builder = new StringBuilder();
            this.WriteHeader(builder, filters[0], sceneid, filters.Count, cdfmargin, noise, algos);
            foreach (ParticleFilter filter in filters)
            {
                for (int cycleIndex = 1; cycleIndex <= cycleamount; cycleIndex++)
                {
                    this.AddResults(filter.CalculatePose(cycleIndex));
                }
            }

            this.WriteResults(builder, cycleamount);
            this.WriteActual(builder, posscen, oriscen, cycleamount);

            int particleamount = filters[0].Particles.RowCount;
            StringBuilder namebuilder = new StringBuilder();
            namebuilder.AppendFormat(
                "P{0}_CDF{1}_N{2}_A{3}_S{4}_C{5}.csv",
                particleamount,
                Math.Round(1 / cdfmargin),
                Math.Round(1 / noise),
                algos,
                sceneid,
                filters.Count);
            string path = @"D:\Users\Yoeri 2\Documenten\MATLAB\FilterAnalyse\Data\";
            string filepath = Path.GetFullPath(path + namebuilder);
            File.WriteAllText(filepath, builder.ToString());
        }

        private void AddResults(Pose pose)
        {
            this.posx.Add(pose.Position.X);
            this.posy.Add(pose.Position.Y);
            this.posz.Add(pose.Position.Z);
            this.orix.Add(pose.Orientation.X);
            this.oriy.Add(pose.Orientation.Y);
            this.oriz.Add(pose.Orientation.Z);
        }

        private void Write1Actual(StringBuilder builder, Func<long, float> func, int cycleamount)
        {
            for (int j = 1; j <= cycleamount; j++)
            {
                builder.AppendFormat("{0}, ", func(j));
            }

            builder.AppendLine();
        }

        private void Write3Actual(StringBuilder builder, AbstractScenario3D scen, int cycleamount)
        {
            this.Write1Actual(builder, scen.RealX, cycleamount);
            this.Write1Actual(builder, scen.RealY, cycleamount);
            this.Write1Actual(builder, scen.RealZ, cycleamount);
        }

        private void WriteActual(
            StringBuilder builder,
            PositionScenario posscen,
            OrientationScenario oriscen,
            int cycleamount)
        {
            this.Write3Actual(builder, posscen, cycleamount);
            this.Write3Actual(builder, oriscen, cycleamount);
        }

        private void WriteHeader(
            StringBuilder builder,
            ParticleFilter filter,
            int sceneid,
            int cycles,
            double cdfmargin,
            double noise,
            int algos)
        {
            float rangex = filter.Fieldsize.Xmax - filter.Fieldsize.Xmin;
            float rangey = filter.Fieldsize.Ymax - filter.Fieldsize.Ymin;
            float rangez = filter.Fieldsize.Zmax - filter.Fieldsize.Zmin;
            int particleamount = filter.Particles.RowCount;
            builder.AppendFormat(
                "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                cycles,
                rangex,
                rangey,
                rangez,
                particleamount,
                cdfmargin,
                noise,
                algos,
                sceneid);
            builder.AppendLine();
        }

        private void WriteResult(StringBuilder builder, List<float> list, int cycleamount)
        {
            for (int i = 0; i < this.posx.Count / cycleamount; i++)
            {
                for (int j = 0; j < cycleamount; j++)
                {
                    builder.AppendFormat("{0}, ", list[(i * cycleamount) + j]);
                }

                builder.AppendLine();
            }
        }

        private void WriteResults(StringBuilder builder, int cycleamount)
        {
            this.WriteResult(builder, this.posx, cycleamount);
            this.WriteResult(builder, this.posy, cycleamount);
            this.WriteResult(builder, this.posz, cycleamount);
            this.WriteResult(builder, this.orix, cycleamount);
            this.WriteResult(builder, this.oriy, cycleamount);
            this.WriteResult(builder, this.oriz, cycleamount);
        }
    }
}