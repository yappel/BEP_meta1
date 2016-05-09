
namespace IRescue.UserLocalisation.Particle
{
    using Sensors;
    using Core.Datatypes;
    using System;
    using System.Collections.Generic;

    public class MonteCarloLocalizer : AbstractUserLocalizer, IMotionReceiver, ILocationReceiver
    {
        public override void ProcessLocation(List<IRVectorDeviation> locations)
        {
            throw new NotImplementedException();
        }

        public void RegisterLocationReceiver(ILocationSource source)
        {
            throw new NotImplementedException();
        }

        public void RegisterMotionSource(IMotionSource source)
        {
            throw new NotImplementedException();
        }
    }
}
