using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class ParkingSessionRepository : Repository<ParkingSession, string>, IParkingSessionRepository
    {
        public ParkingSessionRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<ParkingSession?> FindIncludingParkingLot(string id)
        {
            return await _dbSet.Include(ps => ps.ParkingLot)
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<ParkingSession?> FindIncludingParkingLotReadOnly(string id)
        {
            return await _dbSet.Include(ps => ps.ParkingLot)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<ParkingSession?> FindIncludingVehicleAndParkingLot(string id)
        {
            return await _dbSet.Include(ps => ps.Vehicle)
                .Include(ps => ps.ParkingLot)
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<ParkingSession?> FindIncludingVehicleAndParkingLotReadOnly(string id)
        {
            return await _dbSet.Include(ps => ps.Vehicle)
                .Include(ps => ps.ParkingLot)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<ParkingSession?> FindIncludingVehicle(string id)
        {
            return await _dbSet.Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<ParkingSession?> FindIncludingVehicleReadOnly(string id)
        {
            return await _dbSet.Include(ps => ps.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        protected override Expression<Func<ParkingSession, bool>> CreateIdPredicate(string id)
        {
            return ps => ps.Id == id;
        }

        public Task<ParkingSession?> FindIncludingVehicleAndOwner(string id)
        {
           return _dbSet.Include(ps => ps.Vehicle)
                .ThenInclude(v => v.Owner)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public Task<ParkingSession?> FindIncludingVehicleAndOwnerReadOnly(string id)
        {
            return _dbSet.Include(ps => ps.Vehicle)
                .ThenInclude(v => v.Owner)
                .ThenInclude(o => o.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public Task<ParkingSession?> FindInlcudingParkingFeeScheduleReadOnly(string id)
        {
            return _dbSet.Include(ps => ps.ParkingFeeSchedule)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public Task<ParkingSession?> FindLatest(string vehicleId, string parkingLotId)
        {
            return _dbSet.Where(ps => ps.VehicleId == vehicleId && ps.ParkingLotId == parkingLotId)
                .OrderByDescending(ps => ps.EntryDateTime)
                .Include(ps => ps.Vehicle)
                .ThenInclude(v => v.Owner)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountTransactions()
        {
            // Sum the TransactionCount property for all sessions, treating null as 0
            return await _dbSet.SumAsync(ps => ps.TransactionCount ?? 0);
        }
    }
}
