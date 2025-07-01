﻿using System;
using System.Collections.Generic;

namespace SAPLSServer.Models;

public partial class AttachedFile
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime UploadAt { get; set; }

    public virtual IncidenceEvidence? IncidenceEvidence { get; set; }

    public virtual RequestAttachedFile? RequestAttachedFile { get; set; }
}
