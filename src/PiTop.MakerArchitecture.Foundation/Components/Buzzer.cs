using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Components
{
    public class Buzzer : PlateConnectedDevice
    {
        private readonly IGpioControllerFactory _controllerFactory;
        private int _buzzPin;
        private bool _isOn;
        private GpioController Controller { get; set; }

        public Buzzer(IGpioControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        public void On()
        {
            if (!_isOn)
            {
                _isOn = true;
                Controller.Write(_buzzPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn)
            {
                _isOn = false;
                Controller.Write(_buzzPin, PinValue.Low);
            }
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

        public void Toggle()
        {
            IsOn = !IsOn;
        }

        protected override void OnConnection()
        {
            Controller = _controllerFactory.GetOrCreateController();
            if (Port!.PinPair is { } pinPair)
            {
                _buzzPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
            
            AddToDisposables(Controller.OpenPinAsDisposable(_buzzPin, PinMode.Output));
        }
    }
}