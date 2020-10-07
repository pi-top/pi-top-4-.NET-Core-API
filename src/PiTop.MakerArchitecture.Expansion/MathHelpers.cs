﻿namespace PiTop.MakerArchitecture.Expansion
{
    public static class MathHelpers
    {
        public static double Interpolate(this double point, double domainMin, double domainMax, double codomainMin, double codomainMax )
        {
            var clamped = point > domainMax? domainMax : point < domainMin? domainMin: point;

            var pos = (clamped - domainMin)/(domainMax - domainMin);

            var interp = (pos * (codomainMax - codomainMin)) + codomainMin;
            return interp;

        }
    }
}
