using PiTop.Abstractions;
using Pocket;
using System;
using System.IO;
using UnitsNet;
using static Pocket.Logger;

namespace PiTop.MakerArchitecture.Expansion
{
    public class EncoderMotor : PlateConnectedDevice
    {
        private const int MMK_STANDARD_GEAR_RATIO = 42;
        private const int MAX_DC_MOTOR_RPM = 6000;
        private readonly SMBusDevice _controller;

        public EncoderMotorPort Port { get; }
        private byte RegisterControlMode => (byte)(0x60 + Port);
        private byte RegisterMode0Power => (byte)(0x64 + Port);
        private byte RegisterMode1Rpm => (byte)(0x68 + Port);
        private byte RegisterMode2RpmWithRotations => (byte)(0x6C + Port);
        private byte RegisterBrakeType => (byte)(0x70 + Port);
        private byte RegisterTachometer => (byte)(0x75 + Port);
        private byte RegisterOdometer => (byte)(0x7A + Port);
        public ForwardDirection ForwardDirection { get; set; }

        public BrakingType BrakingType
        {
            get => (BrakingType)_controller.ReadByte(RegisterBrakeType);
            set => _controller.WriteByte(RegisterBrakeType, (byte)value);
        }

        private byte ControlMode
        {
            get => _controller.ReadByte(RegisterControlMode);
            set
            {
                using var operation = Log.OnEnterAndExit();
                operation.Info("controllerMode", value);
                _controller.WriteByte(RegisterControlMode, value);
            }
        }

        /// <summary>
        /// Get or set the power value. Must be in the range [-1,1]
        /// </summary>
        public double Power
        {
            get => ControlMode == 0 ? MapMotorPower(_controller.ReadWordSigned(RegisterMode0Power)) : double.NaN;
            set
            {
                ControlMode = 0;
                _controller.WriteWord(RegisterMode0Power, ToMotorPower(value));
            }
        }


        public static RotationalSpeed MaxRpm => RotationalSpeed.FromRevolutionsPerMinute((double)MAX_DC_MOTOR_RPM / MMK_STANDARD_GEAR_RATIO);



        public RotationalSpeed Rpm
        {
            get => ControlMode == 0 ? RotationalSpeed.Zero : ReadRpm();
            set => ReachSpeed(value);
        }

        public RotationalSpeed ActualRpm => ControlMode == 0 ? RotationalSpeed.Zero : ReadActualRpm();

        public EncoderMotor(EncoderMotorPort port, SMBusDevice controller)
        {
            ForwardDirection = ForwardDirection.Clockwise;
            _controller = controller;
            Port = port;
        }

        public double RotationCount
        {
            get
            {
                var data = _controller.Read32(RegisterOdometer);
                var count = BitConverter.ToInt32(data.Slice(0, 4)) * (int)ForwardDirection;
                return Math.Round((double)count / MMK_STANDARD_GEAR_RATIO, 1);
            }
        }


        public void Stop()
        {
            switch (ControlMode)
            {
                case 0:
                    Power = 0;
                    break;
                case 1:
                case 2:
                    ReachSpeed(RotationalSpeed.Zero);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ControlMode", "ControlMode can only be one of 0,1,2");

            }
        }

        private short ToMotorPower(in double value)
        {
            if (Math.Abs(value) > 1)
            {
                throw new ArgumentOutOfRangeException("Power", message: "Power values must be in the range [-1,1]");
            }
            var sign = (short)ForwardDirection;

            return (short)(Math.Round(value * 1000, 2) * sign);
        }

        private double MapMotorPower(short value)
        {
            var sign = (int)ForwardDirection;
            return value / 1000.0 * sign;
        }

        private RotationalSpeed ReadRpm()
        {
            var sign = (int)ForwardDirection;
            short rpm = 0;
            switch (ControlMode)
            {
                case 1:
                    rpm = _controller.ReadWordSigned(RegisterMode1Rpm);
                    break;

                case 2:
                    var data = _controller.Read32(RegisterMode2RpmWithRotations);
                    rpm = BitConverter.ToInt16(data.Slice(2, 2));
                    break;
            }
            return RotationalSpeed.FromRevolutionsPerMinute((double)rpm / (MMK_STANDARD_GEAR_RATIO * sign));

        }

        private RotationalSpeed ReadActualRpm()
        {
            var rpm = MAX_DC_MOTOR_RPM;
            for (var i = 0; i < 3; i++) // retry reading a valid tachometer value a finite number of times
            {
                rpm = _controller.ReadWordSigned(RegisterTachometer);
                if (Math.Abs(rpm) <= MAX_DC_MOTOR_RPM)
                {
                    var sign = (int)ForwardDirection;
                    return RotationalSpeed.FromRevolutionsPerMinute((double)rpm / (MMK_STANDARD_GEAR_RATIO * sign));
                }
            }

            throw new InvalidDataException($"Error reading tachometer, {rpm} is more RPM than the motor is capable of ({MAX_DC_MOTOR_RPM} RPM)");
        }

        private void ReachSpeed(RotationalSpeed speed)
        {
            ControlMode = 1;
            _controller.WriteWord(RegisterMode1Rpm, ToRpm(speed));
        }

        private short ToRpm(RotationalSpeed speed)
        {
            using var operation = Log.OnExit();

            var sign = (int)ForwardDirection;

            var value = (short)(Math.Round(speed.RevolutionsPerMinute * MMK_STANDARD_GEAR_RATIO, 2) * sign);
            operation.Info("RPM", value);
            return value;
        }

        /// <inheritdoc />
        protected override void OnConnection()
        {
            BrakingType = BrakingType.Coast;
            ControlMode = 0;
            Stop();
        }
    }
}