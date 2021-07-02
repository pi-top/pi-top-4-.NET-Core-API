using System;
using System.Collections.Generic;
using System.Linq;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    public class FoundationPlate : PiTopPlate
    {
       

        public FoundationPlate(PiTop4Board module) : base(module)
        {
            RegisterPorts<DigitalPort>();
            RegisterPorts<AnaloguePort>();


        }


        public override IEnumerable<IConnectedDevice> Devices => ConnectedDevices.OfType<IConnectedDevice>();
    }
}