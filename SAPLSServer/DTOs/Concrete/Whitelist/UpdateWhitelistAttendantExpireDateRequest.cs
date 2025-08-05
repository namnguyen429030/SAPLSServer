﻿using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateWhiteListAttendantExpireDateRequest
    {
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string ParkingLotId { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.CLIENT_ID_REQUIRED)]
        public string ClientId { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; }
    }
}
