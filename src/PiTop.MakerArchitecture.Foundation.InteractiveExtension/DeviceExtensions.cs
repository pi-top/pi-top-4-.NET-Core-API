using System;

using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;

using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTop.MakerArchitecture.Foundation.InteractiveExtension
{
    internal static class SvgUtilities
    {
        internal const string TextStyle = "font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;";

        public static PocketView CreatePath(string pathData, string? style = null)
        {
            var path = _.path[d: pathData, style: style ?? ""]();
            return path;
        }
    }

    public static class DeviceExtensions
    {

        internal static IHtmlContent DrawSvg(this PlateConnectedDevice digitalDevice)
        {
            var id = "PiTop.MakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return div[id: id](svg(digitalDevice.GetSvg()));
        }

        /// <summary>
        /// Gets the device value.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">device</exception>
        public static object GetDeviceValue(this PlateConnectedDevice device)
        {
            switch (device)
            {
                case Buzzer buzzer:
                    return buzzer.IsOn ? 1 : 0;
                case Led led:
                    return led.IsOn ? 1 : 0;
                case Button button:
                    return button.IsPressed ? 1 : 0;
                case UltrasonicSensor ultrasonicSensor:
                    return ultrasonicSensor.Distance.Value;
                case LightSensor lightSensor:
                    return lightSensor.Value;
                case Potentiometer potentiometer:
                    return potentiometer.Position;
                case SoundSensor soundSensor:
                    return soundSensor.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(device));
            }
        }
    }
}