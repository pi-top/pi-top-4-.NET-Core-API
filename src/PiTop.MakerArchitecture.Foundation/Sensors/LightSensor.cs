using System;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class LightSensor : AnalogueDeviceBase
    {
        private readonly bool _normalizeValue;
        private AnalogueDigitalConverter _adc;

        public LightSensor(int deviceAddress, II2CDeviceFactory i2CDeviceFactory) : this(deviceAddress, i2CDeviceFactory, true)
        {
        }

        public LightSensor(int deviceAddress, II2CDeviceFactory i2CDeviceFactory, bool normalizeValue = true) : base(deviceAddress, i2CDeviceFactory)
        {
            _normalizeValue = normalizeValue;
        }

        public double Value => ReadValue();

        private double ReadValue()
        {
            var value = _adc.ReadSample(numberOfSamples: 3);
            return Math.Round(_normalizeValue ? value / 999.0 : value, 2);
        }

        /// <inheritdoc />
        protected override void OnConnection()
        {
            if (Port!.PinPair is { } pinPair)
            {
                var bus = I2CDeviceFactory.GetOrCreateI2CDevice(DeviceAddress);
                _adc = new AnalogueDigitalConverter(bus, pinPair.pin0);

                AddToDisposables(_adc);
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }

        }
    }
}