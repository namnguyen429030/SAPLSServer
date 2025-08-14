using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class ParkingLotOwnerProfile
{
    public string UserId { get; set; } = null!;

    public string ParkingLotOwnerId { get; set; } = null!;

    public string? ApiKey { get; set; }

    public string? ClientKey { get; set; }

    public string? CheckSumKey { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public virtual AdminProfile CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

    public virtual ICollection<PaymentSource> PaymentSources { get; set; } = new List<PaymentSource>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual AdminProfile UpdatedByNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
