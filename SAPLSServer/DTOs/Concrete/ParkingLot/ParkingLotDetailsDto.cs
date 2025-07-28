using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingLotDetailsDto : ParkingLotSummaryDto
    {
        public string? Description { get; set; }
        public int TotalParkingSlot { get; set; }

        public ParkingLotDetailsDto() { }

        public ParkingLotDetailsDto(ParkingLot model) : base(model)
        {
            Description = model.Description;
            TotalParkingSlot = model.TotalParkingSlot;
        }
    }
}
