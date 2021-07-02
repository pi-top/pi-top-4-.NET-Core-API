using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using PiTop.Abstractions;
using Pocket;
using UnitsNet;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class UltrasonicSensorGpio : UltrasonicSensor
    {
        private readonly IGpioControllerFactory _controllerFactory;
        private const int MAX_DISTANCE = 300;
        private int _echoPin;
        private int _triggerPin;
        private readonly Stopwatch _timer = new();

        private int _lastMeasurement = 0;
        private ManualResetEvent _echoReceived;
        private GpioController Controller { get; set; }

        public UltrasonicSensorGpio(IGpioControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        /// <inheritdoc />
        protected override void OnConnection()
        {
            Controller = _controllerFactory.GetOrCreateController();
            if (Port!.PinPair is {} pinPair)
            {
                _echoPin = pinPair.pin0;
                _triggerPin = pinPair.pin1;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
           
            _echoReceived = new ManualResetEvent(false);
            AddToDisposables(_echoReceived);

            AddToDisposables(Controller.OpenPinAsDisposable(_echoPin, PinMode.Input));
            AddToDisposables(Controller.OpenPinAsDisposable(_triggerPin, PinMode.Output));
            Controller.Write(_triggerPin, PinValue.Low);
            Controller.Read(_echoPin);
            AddToDisposables(Controller.RegisterCallbackForPinValueChangedEventAsDisposable(_echoPin, PinEventTypes.Falling | PinEventTypes.Rising, (_, s) =>
            {
                switch (s.ChangeType)
                {
                    case PinEventTypes.Rising:
                        _timer.Start();
                        break;
                    case PinEventTypes.Falling:
                        _timer.Stop();
                        _echoReceived.Set();
                        break;
                }
            }));
        }

        protected override Length GetDistance()
        {

            for (var i = 0; i < 10; i++)
            {
                // Measurements should be 60ms apart, in order to prevent trigger signal mixing with echo signal
                // ref https://components101.com/sites/default/files/component_datasheet/HCSR04%20Datasheet.pdf
                var waitMillis = _lastMeasurement + 60 - Environment.TickCount;
                while (waitMillis > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(waitMillis));
                    waitMillis = _lastMeasurement + 60 - Environment.TickCount;
                }

                if (TryGetDistance(out var result))
                {
                    _lastMeasurement = Environment.TickCount; // ensure that we wait 60ms
                    return Length.FromCentimeters(result);
                }

                _lastMeasurement = Environment.TickCount; // ensure that we wait 60ms
            }

            throw new SensorReadException($"Could not get reading from the sensor on port {Port}");
        }

        private bool TryGetDistance(out double result)
        {
            using var operation = Logger.Log.OnEnterAndConfirmOnExit();

            operation.Info("Trigger starting");
            _timer.Reset();
            _echoReceived.Reset();
            // Trigger input for 10uS to start ranging
            Controller.Write(_triggerPin, PinValue.High);
            Thread.Sleep(TimeSpan.FromMilliseconds(0.01));
            Controller.Write(_triggerPin, PinValue.Low);
            operation.Info("Trigger sent");

            if (!_echoReceived.WaitOne(TimeSpan.FromMilliseconds(100)))
            {
                operation.Error("Timeout waiting for reading, Timeout value 100 ms");
                result = default;
                return false;
            }

            var elapsed = _timer.Elapsed;
            operation.Info($"elapsed {elapsed.TotalMilliseconds:F3} ms");

            // distance = (time / 2) × velocity of sound (34300 cm/s)
            result = elapsed.TotalMilliseconds / 2.0 * 34.3;
            if (result > MAX_DISTANCE)
            {
                // result is more than sensor supports
                // something went wrong
                operation.Error($"Out of range reading : {result} is above threshold {MAX_DISTANCE}");
                result = default;
                return false;
            }

            operation.Info($"distance {result:F1} cm");

            operation.Succeed();
            return true;
        }
    }
}