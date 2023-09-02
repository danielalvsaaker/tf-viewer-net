namespace Parser.Fit.Extensions;

internal static class SemicircleToDegreesExtensions
{
    private const double Multiplier = 180.0 / (2L << 30);

    public static double SemicircleToDegree(this int semicircle) =>
        semicircle * Multiplier;
}