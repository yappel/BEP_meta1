using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Unity.Config
{
    public class WrongDefaultConfigFileException : Exception
    {
        public WrongDefaultConfigFileException(string message)
        : base(message)
        {
        }

        public WrongDefaultConfigFileException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
