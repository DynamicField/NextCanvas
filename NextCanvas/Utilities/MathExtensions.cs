using System;

namespace NextCanvas.Utilities
{
    public static class MathExtensions
    {
        public static double Cap(this double value, double minimum)
        {
            return Math.Max(value, minimum);
        }
    }
}