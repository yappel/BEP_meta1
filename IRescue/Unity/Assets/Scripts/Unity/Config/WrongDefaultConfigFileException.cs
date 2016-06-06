namespace Assets.Scripts.Unity.Config
{
    using System;

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
