using System;

using FluentAssertions;
using PiTop.MakerArchitecture.Foundation;
using PiTop.Tests;
using UnitsNet;
using Xunit;

namespace PiTop.MakerArchitecture.Expansion.Tests
{
    public class MmkPlateTests : IDisposable
    {
        private readonly PiTop4Board _module;
        public MmkPlateTests()
        {
            PiTop4Board.Configure(new DummyGpioController(), i2cDeviceFactory:settings => new DummyI2CDevice(settings));
            _module = PiTop4Board.Instance;
        }

        [Fact]
        public void can_obtain_plate_from_module()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void can_obtain_plate_from_module_as_foundation_plate()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();
            var foundationPlate = _module.GetOrCreatePlate<FoundationPlate>();

            foundationPlate.Should().BeSameAs(plate);
        }

        [Fact]
        public void plate_can_create_servo()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var servoMotor = plate.GetOrCreateServoMotor(ServoMotorPort.S1);

            servoMotor.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_create_motor()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var encoderMotor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M1);

            encoderMotor.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1.1f)]
        [InlineData(-1.1f)]
        public void encoderMotor_does_not_accept_power_out_of_range(float power)
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var encoderMotor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M1);
            var action = new Action(() =>
            {
                encoderMotor.Power = power;
            });

            action.Should().Throw<ArgumentException>()
                .Which
                .Message.Should().Be("Power values must be in the range [-1,1] (Parameter 'Power')");
        }

        public void Dispose()
        {
            _module.Dispose();
        }
    }
}