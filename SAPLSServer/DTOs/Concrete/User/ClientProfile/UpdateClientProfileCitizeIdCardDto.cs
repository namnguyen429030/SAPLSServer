using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User.ClientProfile
{
    public class UpdateClientProfileCitizeIdCardDto : UpdateDto
    {
        [Required(ErrorMessage = "Citizen ID card image URL is required.")]
        public string CitizenIdCardImageUrl { get; set; } = null!;
    }
}
