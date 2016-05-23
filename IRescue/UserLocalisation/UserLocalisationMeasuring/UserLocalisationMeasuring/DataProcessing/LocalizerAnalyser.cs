// <copyright file="LocalizerAnalyser.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisationMeasuring.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
            StringBuilder particlebuilder = new StringBuilder();
            StringBuilder weightbuilder = new StringBuilder();
            StringBuilder measurementbuilder = new StringBuilder();

            long totaltime = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (ParticleFilter filter in filters)
            {
                for (int cycleIndex = 1; cycleIndex <= cycleamount; cycleIndex++)
                {
                    stopwatch.Restart();
                    Pose pose = filter.CalculatePose(cycleIndex);
                    totaltime += stopwatch.ElapsedMilliseconds;
                    this.AddResults(pose);
                    this.AddDebugData(filter, particlebuilder, weightbuilder, measurementbuilder, cycleIndex);
                }
            }

            float averagetime = (float)totaltime / ((float)(filters.Count * cycleamount));
            this.WriteHeader(builder, filters[0], sceneid, filters.Count, cdfmargin, noise, algos, averagetime);
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
            string filepath = Path.GetFullPath(path + "Data_" + namebuilder);
            File.WriteAllText(filepath, builder.ToString());

            //filepath = Path.GetFullPath(path + @"Debug\Particles\" + namebuilder);
            //File.WriteAllText(filepath, particlebuilder.ToString());
            //filepath = Path.GetFullPath(path + @"Debug\Weights\" + namebuilder);
            //File.WriteAllText(filepath, weightbuilder.ToString());
            //filepath = Path.GetFullPath(path + @"Debug\Measurements\" + namebuilder);
            //File.WriteAllText(filepath, measurementbuilder.ToString());
            filepath = Path.GetFullPath(path + @"Meas_" + namebuilder);
            File.WriteAllText(filepath, measurementbuilder.ToString());
        }

        private void AddDebugData(ParticleFilter filter, StringBuilder particlebuilder, StringBuilder weightbuilder, StringBuilder measurementbuilder, int timestamp)
        {
            for (int i = 0; i < filter.Particles.RowCount; i++)
            {
                for (int j = 0; j < filter.Particles.ColumnCount; j++)
                {
                    particlebuilder.AppendFormat("{0},", filter.Particles[i, j]);
                }
                particlebuilder.AppendLine();
            }
            particlebuilder.AppendLine();

            for (int i = 0; i < filter.Weights.RowCount; i++)
            {
                for (int j = 0; j < filter.Weights.ColumnCount; j++)
                {
                    weightbuilder.AppendFormat("{0},", filter.Weights[i, j]);
                }
                weightbuilder.AppendLine();
            }
            weightbuilder.AppendLine();

            for (int i = 0; i < Math.Max(filter.Measurementsori.RowCount, filter.Measurementspos.RowCount); i++)
            {
                measurementbuilder.Append($"{timestamp},");
                for (int j = 0; j < filter.Measurementspos.ColumnCount; j++)
                {
                    measurementbuilder.AppendFormat("{0},", filter.Measurementspos[i, j]);
                }
                for (int j = 0; j < filter.Measurementsori.ColumnCount; j++)
                {
                    measurementbuilder.AppendFormat("{0},", filter.Measurementsori[i, j]);
                }
                measurementbuilder.AppendLine();
            }
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
            int algos,
            float averagetime)
        {
            float rangex = filter.Fieldsize.Xmax - filter.Fieldsize.Xmin;
            float rangey = filter.Fieldsize.Ymax - filter.Fieldsize.Ymin;
            float rangez = filter.Fieldsize.Zmax - filter.Fieldsize.Zmin;
            int particleamount = filter.Particles.RowCount;
            builder.AppendFormat(
                "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                cycles,
                rangex,
                rangey,
                rangez,
                particleamount,
                cdfmargin,
                noise,
                algos,
                sceneid,
                averagetime);
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