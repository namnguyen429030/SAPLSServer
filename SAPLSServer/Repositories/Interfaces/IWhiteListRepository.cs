using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IWhiteListRepository
    {
        Task<IEnumerable<WhiteList>> GetByParkingLotIdAsync(string parkingLotId);
    }
}
