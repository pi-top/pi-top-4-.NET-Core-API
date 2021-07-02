using System;

namespace PiTop
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PinPairAttribute : Attribute
    {
        public int Pin0 { get; }
        public int Pin1 { get; }

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public PinPairAttribute(int pin0, int pin1)
        {
            Pin0 = pin0;
            Pin1 = pin1;
        }
    }
}