namespace Parser.Fit.Extensions;

internal static class SemicircleToDegreesExtensions
{
    public static double SemicircleToDegree(this int semicircle)
    {
        const double multiplier = 180.0 / (2 << 30);

        return semicircle * multiplier;
    }
}