using System;

using Microsoft.DotNet.Interactive.Formatting;

using PiTop.MakerArchitecture.Foundation.Sensors;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;
using static PiTop.MakerArchitecture.Foundation.InteractiveExtension.SvgUtilities;

namespace PiTop.MakerArchitecture.Foundation.InteractiveExtension
{
    internal static partial class PlateConnectedDeviceExtensions
    {
 
        internal static PocketView GetSvg(this SoundSensor soundSensor)
        {
            var value = soundSensor.Value.ToString("0.##");
            return g(
                g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                g[transform: "matrix(1,0,0,1,-764.084,-112.282)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("SOUND")
                    ),
                g[@class: "SoundLevel", transform: "matrix(1,0,0,1,-752.084,-120.282)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle](value)
                    ),
                g[transform: "matrix(1,0,0,1,-905.923,-182.8)"](
                    g[transform: "matrix(0.586987,0,0,0.586987,473.851,113.711)"](
                        _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:3.41px;"]()
                        ),
                    g[transform: "matrix(0.0293774,0,0,0.026825,906.652,185.697)"](
                        CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:rgb(0,24,255);stroke:black;stroke-width:71.1px;")
                        )
                    )
                );
        }

        internal static PocketView GetSvg(this Potentiometer potentiometer)
        {
            var value = potentiometer.Position.ToString("0.##");

            return g(
                g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                g[@class: "Pot", transform: "matrix(1,0,0,1,-859.031,-188)"](
                    CreatePath("M871.086,208.585L872.604,204.441C871.982,203.245 871.63,201.886 871.63,200.446C871.63,195.659 875.516,191.773 880.303,191.773C885.089,191.773 888.976,195.659 888.976,200.446C888.976,205.233 885.089,209.119 880.303,209.119C878.424,209.119 876.684,208.52 875.263,207.504L871.086,208.585Z", "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2px;"),
                    g[transform: "matrix(0.743034,-0.597245,0.56137,0.698402,93.8923,710.93)"](
                        _.rect[x: 971.577, y: 104.115, width: 13.572, height: 3.795, style: "fill:rgb(217,217,217);stroke:black;stroke-width:0.92px;stroke-linecap:square;"]()
                        )
                    ),
                g[transform: "matrix(1,0,0,1,-756.199,-120)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle](value)
                ),
                g[transform: "matrix(1,0,0,1,-756.199,-112.282)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("POT")
                    )
                );
        }

        internal static PocketView GetSvg(this LightSensor lightSensor)
        {
            var value = lightSensor.Value.ToString("0.##");
            return g(
                g[transform: "matrix(0.0834819,0,0,0.0762288,-11.0292,-7.71042)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")
                    ),
                g[transform: "matrix(1,0,0,1,-759.74,-112.282)"](
                        _.text[x: "768.748px", y: "149.76px", style: TextStyle]("LIGHT")
                    ),
                g[@class: "LightLevel", transform: "matrix(1,0,0,1,-751.978,-122.282)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle](value)
                    ),
                g[transform: "matrix(0.730603,0,0,0.730603,-555.122,-89.002)"](
                    _.circle[cx: "788.557", cy: "138.653", r: "8.673", style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2.74px;"]())
                );
        }
    }
}