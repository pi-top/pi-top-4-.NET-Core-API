using System;

using PiTop.Abstractions;

using UnitsNet;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class SoundSensor : AnalogueDeviceBase
    {
        private readonly bool _normalizeValue;
        private AnalogueDigitalConverter _adc;

        public SoundSensor(int deviceAddress, II2CDeviceFactory i2CDeviceFactory) : this( deviceAddress, i2CDeviceFactory, true)
        {
        }

        public SoundSensor(int deviceAddress, II2CDeviceFactory i2CDeviceFactory, bool normalizeValue) : base(deviceAddress, i2CDeviceFactory)
        {
            _normalizeValue = normalizeValue;
          
        }

        public double Value => ReadValue(_normalizeValue);

        public Level Level
        {
            get
            {
                var db = 20 * Math.Log10(ReadValue(true));
                return Level.FromDecibels(db);
            }
        }

        private double ReadValue(bool normalize)
        {
            var value = _adc.ReadSample(peakDetection: true);
            if (normalize)
            {
                value /= ushort.MaxValue;
            }
            return Math.Round(value);
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