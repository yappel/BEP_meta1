using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Inputsensors;

namespace Assets.Scripts.UserLocalisation
{
    class MonteCarloLocalizer : AbstractUserLocalizer, IMotionReceiver, ILocationReceiver
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
