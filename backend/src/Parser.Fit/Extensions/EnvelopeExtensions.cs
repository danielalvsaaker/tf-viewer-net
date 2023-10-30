using NetTopologySuite.Geometries;

namespace Parser.Fit.Extensions;

public static class EnvelopeExtensions
{
    public static Geometry ToGeometry(this Envelope envelope)
    {
        return new GeometryFactory().ToGeometry(envelope);
    }
}