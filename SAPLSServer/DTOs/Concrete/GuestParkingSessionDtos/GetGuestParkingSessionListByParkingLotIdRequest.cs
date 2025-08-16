using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos
{
    /// <summary>
    /// Request model for filtering and sorting guest parking sessions by parking lot.
    /// </summary>
    public class GetGuestParkingSessionListByParkingLotIdRequest : GetListRequest
    {
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string ParkingLotId { get; set; } = null!;

        [EnumDataType(typeof(GuestParkingSessionStatus), ErrorMessage = MessageKeys.INVALID_PARKING_SESSION_STATUS)]
        public string? Status { get; set; }

        [DataType(DataType.Date, ErrorMessage = MessageKeys.INVALID_DATE_FORMAT)]
        public DateOnly? StartEntryDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = MessageKeys.INVALID_DATE_FORMAT)]
        public DateOnly? EndEntryDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = MessageKeys.INVALID_DATE_FORMAT)]
        public DateOnly? StartExitDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = MessageKeys.INVALID_DATE_FORMAT)]
        public DateOnly? EndExitDate { get; set; }
    }
}