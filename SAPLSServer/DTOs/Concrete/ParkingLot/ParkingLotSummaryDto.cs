using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingLotSummaryDto : GetResult
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;

        public ParkingLotSummaryDto() { }

        public ParkingLotSummaryDto(ParkingLot model)
        {
            Id = model.Id;
            Name = model.Name;
            Address = model.Address;
        }
    }
}
