namespace Assets.Scripts.Unity.Config
{
    using System;

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