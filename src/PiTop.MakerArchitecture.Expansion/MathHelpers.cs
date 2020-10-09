﻿using System.Linq;

namespace PiTop.MakerArchitecture.Expansion
{
    public static class MathHelpers
    {
        public static double Interpolate(this double point, double domainMin, double domainMax, double codomainMin, double codomainMax)
        {
            var clamped = point > domainMax ? domainMax : point < domainMin ? domainMin : point;

            var pos = (clamped - domainMin) / (domainMax - domainMin);

            var interp = (pos * (codomainMax - codomainMin)) + codomainMin;
            return interp;

        }

        public static double Interpolate(this short point, double codomainMin, double codomainMax)
        {
            return Interpolate(point, short.MinValue, short.MaxValue, codomainMin, codomainMax);
        }

        public static double Interpolate(this double point, double[] domain, double[] codomain)
        {
            if (point > domain.Last()) return codomain.Last();
            var map = domain.Zip(domain.Skip(1))
                .Zip(codomain.Zip(codomain.Skip(1)))
                .First(mapping => point <= mapping.First.Second);
            return Interpolate(point, map.First.First, map.First.Second, map.Second.First, map.Second.Second);
        }

        public static double WithDeadzone(this double point, double minDomain, double maxDomain, double deadZone)
        {
            return Interpolate(point, new[] { minDomain, -deadZone, deadZone, maxDomain }, new[] { minDomain, 0, 0, maxDomain });
        }
    }
}
