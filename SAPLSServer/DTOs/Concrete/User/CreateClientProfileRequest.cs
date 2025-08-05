using SAPLSServer.Constants;
using SAPLSServer.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class CreateClientProfileRequest : CreateUserRequest
    {
        [Required(ErrorMessage = MessageKeys.CITIZEN_ID_REQUIRED)]
        [VietnameseCitizenId(ErrorMessage = MessageKeys.INVALID_CITIZEN_ID)]
        public string CitizenId { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.DATE_OF_BIRTH_REQUIRED)]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = MessageKeys.SEX_REQUIRED)]
        public bool Sex { get; set; }
        [Required(ErrorMessage = MessageKeys.NATIONALITY_REQUIRED)]
        public string Nationality { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PLACE_OF_ORIGIN_REQUIRED)]
        public string PlaceOfOrigin { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PLACE_OF_RESIDENCE_REQUIRED)]
        public string PlaceOfResidence { get; set; } = null!;
    }
}
