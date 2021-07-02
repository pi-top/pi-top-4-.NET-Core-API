using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using PiTop.Abstractions;

namespace PiTop
{
    public abstract class PiTopPlate : IDisposable
    {
        private SMBusDevice? _mcu;

        private readonly ConcurrentDictionary<string, PlatePort> _connectedPorts =
            new(StringComparer.InvariantCultureIgnoreCase);

        public PiTop4Board PiTop4Board { get; }

         public  int McuI2CAddress { protected set; get; } = 0x04;

        public abstract IEnumerable<IConnectedDevice> Devices { get; }

        /// <summary>
        /// List of device connected tho the plate.
        /// </summary>
        public IEnumerable<PlateConnectedDevice> ConnectedDevices =>
            _connectedPorts.Values.Where(p => p.Device is not null).Select(p => p.Device)!;

        /// <summary>
        /// Plate ports
        /// </summary>
        public IEnumerable<PlatePort> Ports => _connectedPorts.Values;

        protected PiTopPlate(PiTop4Board module)
        {
            PiTop4Board = module ?? throw new ArgumentNullException(nameof(module));
        }

        private readonly CompositeDisposable _disposables = new();

        public void Dispose()
        {
            OnDispose(true);
            _disposables.Dispose();
        }

        public SMBusDevice GetOrCreateMcu()
        {
            return _mcu ??= new SMBusDevice(PiTop4Board.GetOrCreateI2CDevice(McuI2CAddress));
        }

        protected void RegisterPort(PlatePort port)
        {
            if (!_connectedPorts.TryAdd(port.Name, port))
            {
                throw new InvalidOperationException("Port already used");
            }
        }

        /// <summary>
        /// Register ports
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentException"></exception>
        protected void RegisterPorts<T>()
        {
            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                foreach (var name in Enum.GetNames(enumType))
                {
                    var pinPair = GetPinPairs(enumType, name);
                    RegisterPort(new PlatePort(name, pinPair));
                }
            }
            else
            {
                throw new ArgumentException($"{enumType.Name} is not an enum.");
            }

            static (int pin0, int pin1)? GetPinPairs(Type @enum, string enumName)
            {
                var memberInfo = @enum.GetMember(enumName)
                    .FirstOrDefault();

                if (memberInfo is not null)
                {
                    var attribute = memberInfo.GetCustomAttributes<PinPairAttribute>()
                        .SingleOrDefault();
                    
                    if (attribute is null)
                    {
                        return null;
                        
                    }
                    return (attribute.Pin0, attribute.Pin1);
                }

                return null;
            }
        }

        /// <summary>
        /// Creates a connected device using the port.
        /// </summary>
        /// <param name="portName">Port to use for the connection.</param>
        /// <typeparam name="T">Type of the device to create.</typeparam>
        /// <returns>The new device.</returns>
        public T GetOrCreateConnectedDevice<T>(string portName) where T : PlateConnectedDevice, new()
        {
            return GetOrCreateConnectedDevice(portName, () => new T());
        }

        /// <summary>
        /// Creates a connected device using the port.
        /// </summary>
        /// <param name="portName">Port to use for the connection.</param>
        /// <param name="deviceFactory">Factory to create the device.</param>
        /// <typeparam name="T">Type of the device to create.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public T GetOrCreateConnectedDevice<T>(string portName, Func<T> deviceFactory) where T : PlateConnectedDevice
        {
            if (string.IsNullOrWhiteSpace(portName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(portName));
            }

            if (deviceFactory == null)
            {
                throw new ArgumentNullException(nameof(deviceFactory));
            }
            
            if (!_connectedPorts.TryGetValue(portName, out var port))
            {
                throw new InvalidOperationException(
                    $"Port : {portName} not available on plate {GetType().Name}");
            }

            if (port.Device is T d)
            {
                return d;
            }

            var device = deviceFactory();
            device.Connect(port);
            _connectedPorts[portName] = port;

            return device;
        }

        protected void RegisterForDisposal(IDisposable disposable)
        {
            if (disposable == null) throw new ArgumentNullException(nameof(disposable));
            _disposables.Add(disposable);
        }

        protected internal void RegisterForDisposal(Action dispose)
        {
            if (dispose == null) throw new ArgumentNullException(nameof(dispose));
            _disposables.Add(Disposable.Create(dispose));
        }

        protected virtual void OnDispose(bool isDisposing)
        {
            _mcu?.I2c.Dispose();
        }
    }
}