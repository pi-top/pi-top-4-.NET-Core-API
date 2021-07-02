using System;

namespace PiTop
{
    public sealed class  PlatePortInUseException : Exception{
        public PlatePortInUseException(PlatePort port): base($"Port already in use by (another) {port.Device}")
        {
            
        }
    }
}