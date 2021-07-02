using System;
using System.Collections.Generic;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    public interface IFoundationPlate
    {
        IEnumerable<AnaloguePortDeviceBase> AnalogueDevices { get; }
        IEnumerable<IConnectedDevice> Devices { get; }
        
        T GetOrCreateDevice<T>(AnaloguePort port, Func<AnaloguePort, int, II2CDeviceFactory, T> factory, int deviceAddress) where T : AnaloguePortDeviceBase;
        T GetOrCreateDevice<T>(AnaloguePort port) where T : AnaloguePortDeviceBase;
        void DisposeDevice<T>(T device) where T : IConnectedDevice;
    }
}