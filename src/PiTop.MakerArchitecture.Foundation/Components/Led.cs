using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Components
{
    public class Led : PlateConnectedDevice
    {
        private readonly IGpioControllerFactory _controllerFactory;
        private int _ledPin;
        private bool _isOn;

        public Led(IGpioControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        protected override void OnConnection()
        {
            Controller = _controllerFactory.GetOrCreateController();
            if (Port!.PinPair is { } pinPair)
            {
                _ledPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }

            _isOn = false;
            AddToDisposables(Controller.OpenPinAsDisposable(_ledPin, PinMode.Output));
            Controller.Write(_ledPin, PinValue.Low);
        }

        private GpioController Controller { get; set; }

        public Led On()
        {
            if (!_isOn)
            {
                _isOn = true;
                Controller.Write(_ledPin, PinValue.High);
            }

            return this;
        }

        public Led Off()
        {
            if (_isOn)
            {
                _isOn = false;
                Controller.Write(_ledPin, PinValue.Low);
            }
            return this;
        }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (value)
                {
                    On();
                }
                else
                {
                    Off();
                }
            }
        }

        public Led Toggle()
        {
            IsOn = !IsOn;
            return this;
        }
    }
}