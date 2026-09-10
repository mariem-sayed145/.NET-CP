using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public sealed class Driver : Entity
{
    public string FullName { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public bool IsAvailable { get; private set; }

    private Driver()
    {
    }

    public Driver(
        string fullName,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Driver name is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Driver phone number is required.");

        FullName = fullName.Trim();
        PhoneNumber = phoneNumber.Trim();
        IsAvailable = true;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}