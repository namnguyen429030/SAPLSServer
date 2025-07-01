using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class AdminProfile
{
    public string UserId { get; set; } = null!;

    public string AdminId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User User { get; set; } = null!;
}
