using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Reactive.Disposables;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    public abstract class DigitalPortDeviceBase : IConnectedDevice
    {
        private readonly CompositeDisposable _disposables = new();
        public DigitalPort Port { get; }
        protected GpioController Controller { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get; }

        protected DigitalPortDeviceBase(DigitalPort port, IGpioControllerFactory controllerFactory)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException(nameof(controllerFactory));
            }
            DisplayProperties = new List<DisplayPropertyBase>();
            Port = port;
            Controller = controllerFactory.GetOrCreateController();

        }

        protected void AddToDisposables(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Controller.Dispose();
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                IsConnected = true;
                OnConnection();
            }
        }

        public bool IsConnected { get; private set; }

        protected virtual void OnConnection()
        {

        }
    }
}
