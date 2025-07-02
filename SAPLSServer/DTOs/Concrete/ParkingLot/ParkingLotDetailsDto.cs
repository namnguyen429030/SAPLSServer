namespace SAPLSServer.DTOs.Concrete.ParkingLot
{
    public class ParkingLotDetailsDto : ParkingLotSummaryDto
    {
        public string? Description { get; set; }
        public int TotalParkingSlot { get; set; }
    }
}
