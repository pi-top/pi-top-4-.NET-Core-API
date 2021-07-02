using System;
using System.Reactive.Disposables;
using PiTop.Abstractions;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Sensors;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Sensors
{

    public class UltrasonicSensorSMBus: UltrasonicSensor
    {
        private readonly SMBusDevice _controller;
        private byte _configRegister;
        private byte _dataRegister;
        

        public UltrasonicSensorSMBus(SMBusDevice controller)
        {
            _controller = controller;
            AddToDisposables(Disposable.Create(() =>
            {
                _controller.WriteByte(_configRegister, 0x00);
            }));
        }

        /// <inheritdoc />
        protected override Length GetDistance()
        {
            try
            {
                var data = _controller.ReadWord(_dataRegister);
                return Length.FromCentimeters(data);
            }
            catch (Exception e)
            {
                throw new SensorReadException($"Could not get reading from the sensor on port {Port}",e);

            }
        }

        /// <inheritdoc />
        protected override void OnConnection()
        {
            switch (Port!.Name)
            {
                case "A1":
                    _configRegister = 0x0C;
                    _dataRegister = 0x0E;
                    break;

                case "A3":
                    _configRegister = 0x0D;
                    _dataRegister = 0x0F;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Port), Port.Name, "This sensor can only work on ports: A1 and A3");
            }
            _controller.WriteByte(_configRegister, 0xA1);
        }
    }
}