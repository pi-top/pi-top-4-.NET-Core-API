using UnitsNet;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public abstract class UltrasonicSensor : PlateConnectedDevice
    {
        public Length Distance => GetDistance();
        protected abstract Length GetDistance();
    }
}