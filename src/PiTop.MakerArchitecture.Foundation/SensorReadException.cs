using System;

namespace PiTop.MakerArchitecture.Foundation
{
    public class SensorReadException : Exception
    {
        public SensorReadException(string message): base(message)
        {
            
        }

        public SensorReadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}