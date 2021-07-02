using Iot.Device.Imu;
using PiTop.MakerArchitecture.Foundation;

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Expansion
{
    public class ExpansionPlate : FoundationPlate
    {
        
        private Mpu9250? _imu;

        public RotationalSpeed3D AngularVelocity => GetAngularVelocity();

        public Acceleration3D Acceleration => GetAcceleration();

        public Temperature Temperature => GetTemperature();

        public Angle Heading => GetHeading();

        public MagneticField3D MagneticField => GetMagneticField();

        public Orientation3D Orientation => GetOrientation();
        
        private const byte REGISTER_HEARTBEAT = 0x40;
        private static readonly TimeSpan HEARTBEAT_SEND_INTERVAL = TimeSpan.FromSeconds(0.5);
        private const byte HEARTBEAT_SECONDS_BEFORE_SHUTDOWN = 2;

        public ExpansionPlate(PiTop4Board module) : base(module)
        {
           
            RegisterPorts<ServoMotorPort>();
            RegisterPorts<EncoderMotorPort>();


            // set up heartbeat
            RegisterForDisposal(Observable.Interval(HEARTBEAT_SEND_INTERVAL, TaskPoolScheduler.Default).Subscribe(_ =>
            {
                GetOrCreateMcu().WriteByte(REGISTER_HEARTBEAT, HEARTBEAT_SECONDS_BEFORE_SHUTDOWN);
            }));

            RegisterForDisposal(() =>
            {
                _imu?.Dispose();
            });
        }

       


        private Orientation3D GetOrientation()
        {
            var acceleration = GetAcceleration();
            var x = acceleration.X.StandardGravity;
            var y = acceleration.Y.StandardGravity;
            var z = acceleration.Z.StandardGravity;

            var xSquare = x * x;
            var ySquare = y * y;
            var zSquare = z * z;
            return new Orientation3D(
                 Angle.FromRadians(Math.Atan(x / Math.Sqrt(ySquare + zSquare))),
            Angle.FromRadians(Math.Atan(y / Math.Sqrt(xSquare + zSquare))),
            Angle.FromRadians(Math.Atan(z / Math.Sqrt(xSquare + ySquare)))
                );
        }

        private Mpu9250 GetOrCreateMPU9250()
        {
            return _imu ??= CreateAndCalibrate();

            Mpu9250 CreateAndCalibrate()
            {
                var device = new Mpu9250(GetOrCreateMcu().I2c);
                device.CalibrateMagnetometer();
                device.CalibrateGyroscopeAccelerometer();
                return device;
            }
        }

        private Angle GetHeading()
        {
            var magneticField = GetMagneticField();
            return Angle.FromRadians(Math.Atan2(magneticField.Y.Microteslas, magneticField.X.Microteslas));
        }

        private Temperature GetTemperature()
        {
            var imu = GetOrCreateMPU9250();
            return imu.GetTemperature();
        }

        private Acceleration3D GetAcceleration()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.GetAccelerometer();
            return Acceleration3D.FromVector(reading, AccelerationUnit.StandardGravity);
        }

        private MagneticField3D GetMagneticField()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.ReadMagnetometer();
            return MagneticField3D.FromVector(reading, MagneticFieldUnit.Microtesla);
        }

        private RotationalSpeed3D GetAngularVelocity()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.GetGyroscopeReading();
            return RotationalSpeed3D.FromVector(reading, RotationalSpeedUnit.DegreePerSecond);

        }
    }
}