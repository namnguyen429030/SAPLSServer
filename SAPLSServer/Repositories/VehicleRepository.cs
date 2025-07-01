using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class VehicleRepository : Repository<Vehicle, string>, IVehicleRepository
    {
        public VehicleRepository(SaplsContext context) : base(context)
        {
        }

        public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetByOwnerIdAsync(string ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
