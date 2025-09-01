using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class GuestParkingSession
{
    public string Id { get; set; } = null!;

    public string VehicleLicensePlate { get; set; } = null!;

    public string VehicleType { get; set; } = null!;

    public string CheckInStaffId { get; set; } = null!;

    public string? CheckOutStaffId { get; set; }

    public string ParkingLotId { get; set; } = null!;

    public DateTime EntryDateTime { get; set; }

    public DateTime? ExitDateTime { get; set; }

    public DateTime? CheckOutDateTime { get; set; }

    public string EntryFrontCaptureUrl { get; set; } = null!;

    public string EntryBackCaptureUrl { get; set; } = null!;

    public string? ExitFrontCaptureUrl { get; set; }

    public string? ExitBackCaptureUrl { get; set; }

    public string? TransactionId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Cost { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string ParkingFeeSchedule { get; set; } = null!;

    public virtual StaffProfile CheckInStaff { get; set; } = null!;

    public virtual StaffProfile? CheckOutStaff { get; set; }

    public virtual ParkingFeeSchedule ParkingFeeScheduleNavigation { get; set; } = null!;

    public virtual ParkingLot ParkingLot { get; set; } = null!;
}
