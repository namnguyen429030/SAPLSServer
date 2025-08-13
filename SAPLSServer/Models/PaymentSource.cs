using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class PaymentSource
{
    public string Id { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string ParkingLotOwnerId { get; set; } = null!;

    public virtual ParkingLotOwnerProfile ParkingLotOwner { get; set; } = null!;
}
