using PiTop.MakerArchitecture.Expansion.Sensors;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Sensors;

namespace PiTop.MakerArchitecture.Expansion
{
    public static class ExpansionPlateExtensions
    {
        public static ExpansionPlate GetOrCreateExpansionPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<ExpansionPlate>();
        }

        public static ServoMotor GetOrCreateServoMotor(this ExpansionPlate plate, ServoMotorPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new ServoMotor(port, plate.GetOrCreateMcu()));
        }

        public static EncoderMotor GetOrCreateEncoderMotor(this ExpansionPlate plate, EncoderMotorPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), ()=> new EncoderMotor(port, plate.GetOrCreateMcu()));
        }

        public static UltrasonicSensor GetOrCreateUltrasonicSensor(this ExpansionPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new UltrasonicSensorSMBus(plate.GetOrCreateMcu()));
        }
    }
}