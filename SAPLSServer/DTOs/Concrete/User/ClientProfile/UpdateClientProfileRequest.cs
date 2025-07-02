using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class UpdateClientProfileRequest : UpdateUserRequest
    {
        [Required(ErrorMessage = "Citizen ID card image URL is required.")]
        public string CitizenIdCardImageUrl { get; set; } = null!;
        [Required(ErrorMessage = "Client ID is required.")]
        public string CitizenId { get; set; } = null!;
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Sex is required.")]
        public bool Sex { get; set; }
        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } = null!;
        [Required(ErrorMessage = "Place of origin is required.")]
        public string PlaceOfOrigin { get; set; } = null!;
        [Required(ErrorMessage = "Place of residence is required.")]
        public string PlaceOfResidence { get; set; } = null!;
    }
}
