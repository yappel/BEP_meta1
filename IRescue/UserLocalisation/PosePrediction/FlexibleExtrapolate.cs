using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRescue.UserLocalisation.PosePrediction
{
    using IRescue.Core.DataTypes;

    using MathNet.Numerics;
    using MathNet.Numerics.Interpolation;

    class FlexibleExtrapolate : IExtrapolate
    {
        private const int buffersize = 150;

        private SortedList<double, double> data;

        public FlexibleExtrapolate()
        {
            this.data = new SortedList<double, double>();
        }

        public double PredictValueAt(long x)
        {
            IInterpolation interpolation = Interpolate.Linear(this.data.Keys, this.data.Values);
            return interpolation.Interpolate(x);
        }

        public void AddData(long x, double y)
        {
            this.data.Add(x, y);
            //while (this.data.First().Key < x - buffersize)
            //{
            //    this.data.RemoveAt(0);
            //}
            while (this.data.Count > 4)
            {
                this.data.RemoveAt(0);
            }
        }

        public double PredictChange(long xfrom, long xto)
        {
            if (this.data.Count < 3)
            {
                return 0;
            }
            IInterpolation interpolation = Interpolate.Common(this.data.Keys, this.data.Values);
            double yfrom = interpolation.Interpolate(xfrom);
            double yto = interpolation.Interpolate(xto);
            if (this.data.Count > 1)
                System.Console.WriteLine($"Prevprevprev was {this.data.Values[this.data.Count - 3]}: ({this.data.Keys[this.data.Count - 3]}), Prevprev was {this.data.Values[this.data.Count - 2]}: ({this.data.Keys[this.data.Count - 2]}), prev was {this.data.Values[this.data.Count - 1]}: ({this.data.Keys[this.data.Count - 1]}), prediction {yto}: ({xto})");
            return yto - yfrom;
        }
    }
}
