using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Unity.Config
{
    public class KeyNotFoundException : Exception
    {

        public KeyNotFoundException(string message)
        : base(message)
        {
        }

        public KeyNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}