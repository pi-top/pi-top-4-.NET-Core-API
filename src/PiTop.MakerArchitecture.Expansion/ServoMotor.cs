using PiTop.Abstractions;
using System;
using System.Linq;
using PiTop.Algorithms;
using Pocket;
using UnitsNet;
using static Pocket.Logger;

namespace PiTop.MakerArchitecture.Expansion
{
    public class ServoMotor : PlateConnectedDevice
    {
        private readonly SMBusDevice _controller;
        private Angle _zeroPoint;
        private RotationalSpeed _defaultSpeed;
        public ServoMotorPort Port { get; }

        private const int ANGLE_RANGE = 180;
        private const int SPEED_RANGE = 100;
        private const short MIN_PULSE_WIDTH_MICRO_S = 500;
        private const short MAX_PULSE_WIDTH_MICRO_S = 2500;



        private const byte REGISTER_MIN_PULSE_WIDTH = 0x4A;
        private const byte REGISTER_MAX_PULSE_WIDTH = 0x4B;
        private const byte REGISTER_PWM_FREQUENCY = 0x4C;
        private const byte PWM_FREQUENCY = 60;
        private const double PWM_PERIOD = 1.0 / PWM_FREQUENCY;
        private const int DUTY_REGISTER_RANGE = 4095;

        // Calculate the upper and lower bounds for mapping angles to duty cycles
        private readonly int SERVO_LOWER_DUTY = (int)Math.Round(DUTY_REGISTER_RANGE * ((MIN_PULSE_WIDTH_MICRO_S * 1e-6) / PWM_PERIOD));
        private readonly int SERVO_UPPER_DUTY = (int)Math.Round(DUTY_REGISTER_RANGE * ((MAX_PULSE_WIDTH_MICRO_S * 1e-6) / PWM_PERIOD));

        private byte RegisterControlMode => (byte)(0x50 + Port);
        private byte RegisterSpeed => (byte)(0x56 + Port);
        private byte RegisterAngleAndSpeed => (byte)(0x5C + Port);

        public ServoMotor(ServoMotorPort port, SMBusDevice controller)
        {
            _controller = controller;
            Port = port;
            ZeroPoint = Angle.Zero;
            _defaultSpeed = RotationalSpeed.FromDegreesPerSecond(50);
        }

        /// <summary>
        /// Set the control mode of the servo. In control mode 0, the servo will move at the provided speed until it reaches
        /// a limit. In control mode 1, the servo will move at a provided speed to a provided angle.
        /// </summary>
        private byte ControlMode
        {
            get => _controller.ReadByte(RegisterControlMode);
            set => _controller.WriteByte(RegisterControlMode, value);
        }

        public Angle ZeroPoint
        {
            get => _zeroPoint;
            set
            {
                if (value.Degrees < -90 || value.Degrees > 90)
                {
                    throw new ArgumentOutOfRangeException(nameof(ZeroPoint), "ZeroPoint must be an angle between -90 and 90 degrees");
                }
                _zeroPoint = value;

            }
        }

        public void GoToAngle(Angle angle)
        {
            GoToAngle(angle, _defaultSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="speed"></param>
        public void GoToAngle(Angle angle, RotationalSpeed speed)
        {
            using var operation = Log.OnExit();
            if (Math.Abs((angle + ZeroPoint).Degrees) > ANGLE_RANGE / 2)
            {
                throw new ArgumentOutOfRangeException(nameof(angle),
                    $"Angle value must be in range [-{ANGLE_RANGE},{ANGLE_RANGE}] degrees, taking into account the current ZeroPoint ({ZeroPoint.Degrees} degrees).");
            }

            if (Math.Abs(speed.DegreesPerSecond) > SPEED_RANGE)
            {
                throw new ArgumentOutOfRangeException(nameof(speed),
                    $"Speed value must be in range [-{SPEED_RANGE},{SPEED_RANGE}] degrees/second.");
            }


            var dutyCycle = (short) Math.Round(Interpolation.Interpolate((angle + ZeroPoint).Degrees,
                -ANGLE_RANGE / 2, ANGLE_RANGE / 2, SERVO_LOWER_DUTY, SERVO_UPPER_DUTY));
            var s = (short) (Math.Round(speed.DegreesPerSecond * 10));

            ControlMode = 1;

            _controller.Write32(RegisterAngleAndSpeed, dutyCycle, s);

            operation.Info(
                "Setting Servo angle to {angle} with speed {speed}, by pushing dutyCycle {dutyCycle} and speed {s} ",
                angle, speed, dutyCycle, s);
        }

        /// <summary>
        /// The speed with which to move to the limit of the servo motor, from -100.0 to 100.0
        /// </summary>
        public RotationalSpeed Speed
        {
            get =>
                RotationalSpeed.FromDegreesPerSecond(
                    Math.Round((double)_controller.ReadWordSigned(RegisterSpeed) / 10, 1));
            set
            {
                if (Math.Abs(value.DegreesPerSecond) > SPEED_RANGE)
                {
                    throw new ArgumentException("Servo speed must be between -100.0 and 100.0");
                }

                var speed = (short)Math.Round(value.DegreesPerSecond * 10, 0);

                ControlMode = 0;
                _controller.WriteWord(RegisterSpeed, speed);
            }
        }

  

        private void Stop()
        {
            Speed = RotationalSpeed.Zero;
        }


        /// <inheritdoc />
        protected override void OnConnection()
        {
            _controller.WriteWord(REGISTER_MIN_PULSE_WIDTH, MIN_PULSE_WIDTH_MICRO_S);
            _controller.WriteWord(REGISTER_MAX_PULSE_WIDTH, MAX_PULSE_WIDTH_MICRO_S);
            _controller.WriteByte(REGISTER_PWM_FREQUENCY, PWM_FREQUENCY);
        }
    }
}