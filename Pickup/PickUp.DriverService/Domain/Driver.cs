using NetTopologySuite.Geometries;
using PickUp.Common.Domain.BaseModels;

namespace PickUp.DriverService.Domain
{
    public class Driver : BaseChangeTrackingEntityWithCorrelation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DriverStatus Status { get; set; }
        public Guid CurrentTripId { get; set; }
        public Point? Location { get; set; }
        public int Version { get; set; }
    }

    public enum DriverStatus
    {
        Offline,
        Available,
        Bussy
    }
}
