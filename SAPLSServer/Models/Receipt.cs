using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class Receipt
{
    public int Id { get; set; }

    public int Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Note { get; set; }

    public string ParkingLotOwnerUserId { get; set; } = null!;

    public virtual ParkingLotOwnerProfile ParkingLotOwnerUser { get; set; } = null!;
}
