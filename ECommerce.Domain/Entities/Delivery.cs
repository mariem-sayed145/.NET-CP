using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Delivery : Entity
{
    public int OrderId { get; private set; }

    public Order? Order { get; private set; }

    public int? DriverId { get; private set; }

    public Driver? Driver { get; private set; }

    public DeliveryStatus Status { get; private set; }

    public double? CurrentLatitude { get; private set; }

    public double? CurrentLongitude { get; private set; }

    public DateTime? EstimatedDeliveryTime { get; private set; }

    public DateTime? AssignedAt { get; private set; }

    public DateTime? PickedUpAt { get; private set; }

    public DateTime? DeliveredAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Delivery()
    {
    }

    public Delivery(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Invalid Order ID.");

        OrderId = orderId;
        Status = DeliveryStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignDriver(int driverId)
    {
        if (driverId <= 0)
            throw new ArgumentException("Invalid Driver ID.");

        DriverId = driverId;
        Status = DeliveryStatus.Assigned;
        AssignedAt = DateTime.UtcNow;
    }

    public void PickUp()
    {
        if (Status != DeliveryStatus.Assigned)
            throw new InvalidOperationException(
                "Delivery must be assigned before pickup.");

        Status = DeliveryStatus.PickedUp;
        PickedUpAt = DateTime.UtcNow;
    }

    public void StartDelivery(DateTime? estimatedDeliveryTime = null)
    {
        if (Status != DeliveryStatus.PickedUp)
            throw new InvalidOperationException(
                "Delivery must be picked up before starting delivery.");

        Status = DeliveryStatus.OutForDelivery;
        EstimatedDeliveryTime = estimatedDeliveryTime;
    }

    public void UpdateLocation(
        double latitude,
        double longitude)
    {
        if (latitude is < -90 or > 90)
            throw new ArgumentException("Invalid latitude.");

        if (longitude is < -180 or > 180)
            throw new ArgumentException("Invalid longitude.");

        CurrentLatitude = latitude;
        CurrentLongitude = longitude;
    }

    public void MarkAsDelivered()
    {
        if (Status != DeliveryStatus.OutForDelivery)
            throw new InvalidOperationException(
                "Delivery must be out for delivery first.");

        Status = DeliveryStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = DeliveryStatus.Failed;
    }
}