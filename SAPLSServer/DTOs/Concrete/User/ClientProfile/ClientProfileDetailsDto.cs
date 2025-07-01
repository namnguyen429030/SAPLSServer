﻿using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class ClientProfileDetailsDto : UserDetailsDto
    {
        public string CitizenId { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool Sex { get; set; }
        public string Nationality { get; set; } = null!;
        public string PlaceOfOrigin { get; set; } = null!;
        public string PlaceOfResidence { get; set; } = null!;
    }
}
