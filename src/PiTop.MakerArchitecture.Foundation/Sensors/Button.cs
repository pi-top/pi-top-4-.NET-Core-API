using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class Button : PlateConnectedDevice
    {
        private readonly IGpioControllerFactory _controllerFactory;
        public event EventHandler<bool>? PressedChanged;
        public event EventHandler<EventArgs>? Pressed;
        public event EventHandler<EventArgs>? Released;

        private GpioController Controller { get; set; }

        public Button(IGpioControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        protected override void OnConnection()
        {
            var buttonPin = -1;
            Controller = _controllerFactory.GetOrCreateController();
            if (Port!.PinPair is { } pinPair)
            {
                buttonPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
            
            var openPinAsDisposable = Controller.OpenPinAsDisposable(buttonPin, PinMode.Input);
            var registerCallbackForPinValueChangedEventAsDisposable = Controller.RegisterCallbackForPinValueChangedEventAsDisposable(buttonPin, PinEventTypes.Falling | PinEventTypes.Rising, Callback);

            AddToDisposables(registerCallbackForPinValueChangedEventAsDisposable);
            AddToDisposables(openPinAsDisposable);

            IsPressed = Controller.Read(buttonPin) == PinValue.Low;
        }
        private void Callback(object? _, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            switch (pinValueChangedEventArgs.ChangeType)
            {

                case PinEventTypes.Rising:
                    IsPressed = false;
                    Released?.Invoke(this, EventArgs.Empty);
                    break;
                case PinEventTypes.Falling:
                    IsPressed = true;
                    Pressed?.Invoke(this, EventArgs.Empty);
                    break;

            }
            PressedChanged?.Invoke(this, IsPressed);
        }

        public bool IsPressed { get; set; }
    }
}
