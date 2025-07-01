using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>> GetByOwnerIdAsync(string ownerId);
    }
}
