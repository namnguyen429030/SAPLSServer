using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class ParkingFeeScheduleRepository : Repository<ParkingFeeSchedule, string>, IParkingFeeScheduleRepository
    {
        public ParkingFeeScheduleRepository(SaplsContext context) : base(context)
        {
        }
    }
}
