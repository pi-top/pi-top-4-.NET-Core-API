using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.MakerArchitecture.Foundation
{
    public static class FoundationExtensions
    {
        public static FoundationPlate GetOrCreateFoundationPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<FoundationPlate>();
        }

        public static SoundSensor GetOrCreateSoundSensor(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new SoundSensor(plate.McuI2CAddress, plate.PiTop4Board));
        }

        public static LightSensor GetOrCreateLightSensor(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new LightSensor(plate.McuI2CAddress, plate.PiTop4Board));
        }

        public static Potentiometer GetOrCreatePotentiometer(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Potentiometer(plate.McuI2CAddress,plate.PiTop4Board));
        }

        public static Button GetOrCreateButton(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Button(plate.PiTop4Board));
        }

        public static Buzzer GetOrCreateBuzzer(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Buzzer(plate.PiTop4Board));
        }

        public static UltrasonicSensor GetOrCreateUltrasonicSensor(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new UltrasonicSensorGpio(plate.PiTop4Board));
        }

        public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Led(plate.PiTop4Board));
        }

        public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port, Color displayColor)
        {
            var led = plate.GetOrCreateLed(port);
            var p = displayColor.ToPixel<Argb32>();
            var alpha = p.A / 255.0;
            led.DisplayProperties.Add(new RgbaCssColor(p.R, p.G, p.B, alpha));
            return led;
        }
    }
}