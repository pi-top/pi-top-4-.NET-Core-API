using System;
using System.Linq;

using Microsoft.DotNet.Interactive.Formatting;

using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;
using static PiTop.MakerArchitecture.Foundation.InteractiveExtension.SvgUtilities;

namespace PiTop.MakerArchitecture.Foundation.InteractiveExtension
{
    internal static partial class PlateConnectedDeviceExtensions
    {
        internal static PocketView GetSvg(this PlateConnectedDevice device)
        {
            switch (device)
            {
                case Buzzer buzzer:
                    return buzzer.GetSvg();
                case Led led:
                    return led.GetSvg();
                case Button button:
                    return button.GetSvg();
                case UltrasonicSensor ultrasonicSensor:
                    return ultrasonicSensor.GetSvg();
                case LightSensor lightSensor:
                    return lightSensor.GetSvg();
                case Potentiometer potentiometer:
                    return potentiometer.GetSvg();
                case SoundSensor soundSensor:
                    return soundSensor.GetSvg();
                default:
                    throw new ArgumentOutOfRangeException(nameof(device));
            }
        }

        internal static PocketView GetSvg(this Buzzer buzzer)
        {
            var isOn = buzzer.IsOn;
            var wavesBlock = isOn
                ? g()
                : g[@class: "Waves"](
                    CreatePath("M933.18,189.115C936.844,191.266 939.306,195.248 939.306,199.8C939.306,204.46 936.726,208.522 932.919,210.635", "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2px;"),
                    CreatePath("M920.686,210.498C917.011,208.349 914.54,204.361 914.54,199.8C914.54,195.347 916.896,191.44 920.429,189.258", "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2px;"),
                    g[transform: "matrix(1.27179,0,0,1.27179,-251.928,-54.3035)"](
                        CreatePath("M920.686,210.498C917.011,208.349 914.54,204.361 914.54,199.8C914.54,195.347 916.896,191.44 920.429,189.258", "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:1.57px;")
                        ),
                    g[transform: "matrix(-1.27179,0.000527782,-0.000527782,-1.27179,2105.88,453.415)"](
                        CreatePath("M920.686,210.498C917.011,208.349 914.54,204.361 914.54,199.8C914.54,195.347 916.896,191.44 920.429,189.258", "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:1.57px;")
                        )
                    );

            return g(
                    g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                        CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                    g[transform: "matrix(1,0,0,1,-764.084,-112.282)"](
                        _.text[x: "768.748px", y: "149.76px", style: TextStyle]("BUZZER")
                        ),
                    g[transform: "matrix(1,0,0,1,-905.923,-182.8)"](
                        wavesBlock,
                        g(
                            _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2px;"]())
                        )
                );

        }

        internal static PocketView GetSvg(this Led led)
        {
            var isOn = led.IsOn;
            var color = led.DisplayProperties.OfType<CssColor>().FirstOrDefault();
            var cssColor = color?.Value ?? "rgb(42,236,14)";
            var filledStyle = isOn ? $"fill:{cssColor};" : "fill:rgba(0,0,0,0);";
            return g(
                g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                g[transform: "matrix(1,0,0,1,-767.557,-121.977)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(255,255,255);fill-opacity:0;stroke:black;stroke-width:2px;"]()
                    ),
                g[@class: "Light", transform: "matrix(0.689751,0,0,0.689751,-522.908,-78.9604)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: filledStyle]()
                    ),
                g[transform: "matrix(1,0,0,1,-755.88,-112.184)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("LED"))
                );
        }

        internal static PocketView GetSvg(this Button button)
        {
            var isPressed = button.IsPressed;
            var fillStyle = isPressed ? "fill:rgb(0,0,0);fill-opacity:0;stroke:black;stroke-width:2px;" : "fill:rgb(255,255,255);fill-opacity:0;stroke:black;stroke-width:2px;";
            return g(
                g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                g[transform: "matrix(1,0,0,1,-764.084,-112.282)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("BUTTON")
                    ),
                g[@class: "ButtonState", transform: "matrix(1,0,0,1,-767.557,-121.977)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: fillStyle]()
                    )
                );
        }

        internal static PocketView GetSvg(this UltrasonicSensor ultrasonicSensor)
        {
            var value = ultrasonicSensor.Distance.ToString("0.##");
            return g(
                g[transform: "matrix(0.159967,0,0,0.0762288,-22.0502,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 604.279,114.267 580.926,114.267L186.406,114.267C163.053,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 163.053,639.003 186.406,639.003L580.926,639.003C604.279,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:15.96px;")
                    ),
                g[transform: "matrix(1,0,0,1,-756.787,-111.913)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("ULTRASOUND")
                    ),
                g[@class: "UltrasoundLevel", transform: "matrix(1,0,0,1,-756.787,-120.913)"](
                    _.text[x: "785px", y: "149.76px", style: TextStyle](value)
                    ),
                g[transform: "matrix(1.26023,0,0,1.26023,-975.766,-158.437)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:1.59px;"]()
                    ),
                g[transform: "matrix(1.26023,0,0,1.26023,-933.441,-158.437)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:1.59px;"]()
                    )
                );
        }
    }
}