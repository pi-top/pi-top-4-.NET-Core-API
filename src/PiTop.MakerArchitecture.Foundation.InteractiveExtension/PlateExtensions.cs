using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;


using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTop.MakerArchitecture.Foundation.InteractiveExtension
{
    public static class PlateExtensions
    {
        public static Dictionary<string,object> ToDictionary(this FoundationPlate plate)
        {
            var root = new Dictionary<string,object>();

            foreach (var piTopComponent in plate.ConnectedDevices)
            {
                root[piTopComponent.Port!.Name] = new Dictionary<string, object>
                {
                    {"type", piTopComponent.GetType().Name },
                    {"value", piTopComponent.GetDeviceValue() },
                };
                break;
                
            }

            return root;
        }

        internal static IHtmlContent DrawSvg(this FoundationPlate plate)
        {
            var id = "PiTop.MakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return div[id: id](
                svg[viewBox: "0 0 800 600", width: "100%", height: "100%"](
                    g[transform: "matrix(0.8,0,0,0.8,100,40)"](
                        plate.GetPlateSvg(),
                        plate.GetWiresSvg(),
                        plate.GetDevicesSvg()
                        )
                    )
                );
        }

        internal static PocketView GetWiresSvg(this FoundationPlate plate)
        {
            var lineStyle = "fill:none;stroke:black;stroke-width:11px;stroke-linecap:square;";
            var svgWires = new List<PocketView>();
            foreach (var connectedDevice in plate.ConnectedDevices)
            {
                switch (connectedDevice.Port!.Name)
                {
                    case "D0":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-353.998,-192.173)"](
                            _.path[d: "M840.189,439.958L936.223,440.495L1014.66,455.647L1061.13,455.215", style: lineStyle]()
                        ));
                        break;
                    case "D1":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-353.998,-232.278)"](
                            _.path[d: "M840.189,439.958L935.591,440.491L1014.66,449.524L1061.13,451.186", style: lineStyle]()
                        ));
                        break;
                    case "D2":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-353.998,-272.646)"](
                            _.path[d: "M840.189,439.958L935.782,440.493L1014.66,435.03L1061.13,435.148", style: lineStyle]()
                        ));
                        break;
                    case "D3":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-353.998,-313.785)"](
                            _.path[d: "M840.189,439.958L934.19,440.484L1014.66,434.353L1061.13,433.246", style: lineStyle]()
                        ));
                        break;
                    case "D4":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-314.91)"](
                            _.path[d: "M825.429,435.727L862.406,435.727L945.578,440.547L1041.39,441.083", style: lineStyle]()
                        ));
                        break;
                    case "D5":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-274.674)"](
                            _.path[d: "M825.429,438.876L865.328,439.149L944.183,440.54L1041.39,441.083", style: lineStyle]()
                        ));
                        break;
                    case "D6":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-233.539)"](
                            _.path[d: "M825.429,449.825L863.58,449.319L945.966,440.549L1041.39,441.083", style: lineStyle]()
                        ));
                        break;

                    case "D7":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-193.297)"](
                            _.path[d: "M825.429,452.968L863.85,452.602L945.425,440.546L1041.39,441.083", style: lineStyle]()
                        ));
                        break;
                    case "A0":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1, 0, 0, 1, -353.998, -353.235)"](
                            _.path[d: "M840.189,439.958L934.715,440.487L1014.66,425.698L1061.13,425.662", style: lineStyle]()
                        ));
                        break;
                    case "A1":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-353.998,-394.475)"](
                            _.path[d: "M840.189,439.958L934.749,440.487L1014.66,418.622L1061.13,419.001", style: lineStyle]()
                        ));
                        break;
                    case "A2":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-154.828)"](
                            _.path[d: "M825.429,461.776L866.007,462.122L945.83,440.549L1041.39,441.083", style: lineStyle]()
                        ));
                        break;
                    case "A3":
                        svgWires.Add(g[@class: $"{connectedDevice.Port.Name}Line", transform: "matrix(1,0,0,1,-804.429,-115.255)"](
                            _.path[d: "M827.964,469.271L866.25,467.464L944.568,440.542L1041.39,441.083", style: lineStyle]()
                        ));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return g[@class: "wires_group"](svgWires);
        }

        internal static PocketView GetDevicesSvg(this FoundationPlate plate)
        {
            var svgDevices = new List<PocketView>();
            foreach (var connectedDevice in plate.ConnectedDevices)
            {
                switch (connectedDevice.Port!.Name)
                {
                    case "D0":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,240)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D1":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,192)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D2":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,145)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D3":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,97)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D4":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,94)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D5":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,142)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D6":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,190)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "D7":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,237)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
            
                    case "A0":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,49)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "A1":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,700,2)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "A2":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,285)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                    case "A3":
                        svgDevices.Add(g[@class: $"{connectedDevice.Port.Name}Target", transform: "matrix(1,0,0,1,-11.0292,333)"](
                            connectedDevice.GetSvg()
                        ));
                        break;
                }
            }

            return g[@class: "devices_group"](svgDevices);
        }

        internal static PocketView GetPlateSvg(this FoundationPlate plate)
        {
            var plateSvg = new HtmlString(@"     <g class=""FoundationPlate"" transform=""matrix(1.0428,0,0,1.19407,-290.953,-136.392)"">
        <g transform=""matrix(0.521762,0,0,0.571716,426.226,54.9563)"">
            <path d=""M623.239,126.164C623.239,119.598 616.55,114.267 608.312,114.267L159.02,114.267C150.782,114.267 144.093,119.598 144.093,126.164L144.093,627.106C144.093,633.672 150.782,639.003 159.02,639.003L608.312,639.003C616.55,639.003 623.239,633.672 623.239,627.106L623.239,126.164Z"" style=""fill:white;stroke:black;stroke-width:3.24px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,148.763L623.239,123.481C623.239,118.396 618.005,114.267 611.559,114.267L155.773,114.267C149.327,114.267 144.093,118.396 144.093,123.481L144.093,151.471"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,183.11L144.093,211.495"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,243.134L144.093,268.993"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,300.632L144.093,329.017"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,360.656L144.093,390.015"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,421.653L144.093,450.039"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,481.678L144.093,506.851"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,538.49L144.093,566.875"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M144.093,598.514L144.093,629.788C144.093,634.874 149.327,639.003 155.773,639.003L611.559,639.003C618.005,639.003 623.239,634.874 623.239,629.788L623.239,601.907"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,570.269L623.239,541.883"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,510.245L623.239,481.678"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,450.039L623.239,421.653"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,390.015L623.239,360.656"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,329.017L623.239,300.632"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,268.993L623.239,240.426"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(0.507713,0,0,0.562016,431.616,58.6094)"">
            <path d=""M623.239,208.787L623.239,180.402"" style=""fill:none;stroke:rgb(0,255,154);stroke-width:3.31px;""/>
        </g>
        <g transform=""matrix(2.90042e-16,-4.17189,4.77708,2.53298e-16,-457.883,1561.54)"">
            <text x=""294.077px"" y=""224.915px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">pi-top [4]</text>
            <text x=""281.172px"" y=""233.177px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">foundation plate</text>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,-65.8737)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,-99.6082)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,-133.445)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(20,47,171);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,-167.179)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(20,47,171);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,69.7137)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(255,216,0);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,35.9791)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(255,216,0);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,2.14249)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(0.958955,0,0,0.83747,50.7836,-31.592)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,604.982)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,638.716)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,470.916)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(255,216,0);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,504.651)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(255,216,0);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,670.646)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(20,47,171);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,704.38)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(20,47,171);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,536.966)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(-0.958955,-4.64889e-17,5.32327e-17,-0.83747,1202.03,570.7)"">
            <path d=""M730.613,369.441L713.912,369.441L713.912,390.674L730.613,390.674"" style=""fill:rgb(235,235,235);fill-opacity:0;stroke:rgb(171,20,124);stroke-width:2px;stroke-linecap:square;""/>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-128.974)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D3</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-96.3128)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D2</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-61.9766)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D1</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-29.0497)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D0</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-128.974)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D4</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-96.3128)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D5</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-61.9766)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D6</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-29.0497)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">D7</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-841.435,6.696)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">I2C</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-841.435,39.6228)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">I2C</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-194.941)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">I2C</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,-162.014)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">I2C</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-194.297)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">A1</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-836.641,-161.37)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">A0</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,6.95465)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">A2</text>
        </g>
        <g transform=""matrix(2.26073,0,0,1.97433,-1024.6,39.8815)"">
            <text x=""683.295px"" y=""178.773px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;"">A3</text>
        </g>
    </g>");
            return g[@class: "plate_group"](plateSvg);
        }

    }
}