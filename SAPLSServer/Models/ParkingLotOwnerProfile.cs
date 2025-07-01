using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class ParkingLotOwnerProfile
{
    public string UserId { get; set; } = null!;

    public string ParkingLotOwnerId { get; set; } = null!;

    public virtual ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

    public virtual ICollection<PaymentSource> PaymentSources { get; set; } = new List<PaymentSource>();

    public virtual User User { get; set; } = null!;
}
