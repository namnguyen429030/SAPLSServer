using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class AdminProfile
{
    public string UserId { get; set; } = null!;

    public string AdminId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public virtual AdminProfile? CreatedByNavigation { get; set; }

    public virtual ICollection<AdminProfile> InverseCreatedByNavigation { get; set; } = new List<AdminProfile>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Subscription> SubscriptionCreatedBies { get; set; } = new List<Subscription>();

    public virtual ICollection<Subscription> SubscriptionUpdateBies { get; set; } = new List<Subscription>();

    public virtual User User { get; set; } = null!;
}
