using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace PickUp.Common.Application
{
    public class GeometryFactoryService
    {
        private static readonly int gpsSpatialReferenceId = 4326; //WGS 84
        public static Point CreatePoint(double longitude, double latitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(gpsSpatialReferenceId);

            return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        }
    }
}
