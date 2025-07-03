using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class ParkingLot
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public int TotalParkingSlot { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Settings { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string ParkingLotOwnerId { get; set; } = null!;

    public virtual ICollection<IncidenceReport> IncidenceReports { get; set; } = new List<IncidenceReport>();

    public virtual ICollection<ParkingFeeSchedule> ParkingFeeSchedules { get; set; } = new List<ParkingFeeSchedule>();

    public virtual ParkingLotOwnerProfile ParkingLotOwner { get; set; } = null!;

    public virtual ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();

    public virtual ICollection<ShiftDiary> ShiftDiaries { get; set; } = new List<ShiftDiary>();

    public virtual ICollection<StaffProfile> StaffProfiles { get; set; } = new List<StaffProfile>();

    public virtual ICollection<WhiteList> WhiteLists { get; set; } = new List<WhiteList>();
}
