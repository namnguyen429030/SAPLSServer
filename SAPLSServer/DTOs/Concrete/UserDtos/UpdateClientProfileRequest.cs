﻿using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;
using SAPLSServer.ValidationAttributes;

namespace SAPLSServer.DTOs.Concrete.UserDtos
{
    public class UpdateClientProfileRequest : UpdateUserInformationRequest
    {
        [Required(ErrorMessage = MessageKeys.FRONT_ID_CARD_IMAGE_REQUIRED)]
        [Url(ErrorMessage = MessageKeys.INVALID_FRONT_ID_CARD_IMAGE_UPLOADED)]
        public string FrontIdCardImageUrl { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.BACK_ID_CARD_IMAGE_REQUIRED)]
        [Url(ErrorMessage = MessageKeys.INVALID_BACK_ID_CARD_IMAGE_UPLOADED)]
        public string BackIdCardImageUrl { get; set; } = null!;
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
