using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class ShiftDiaryRepository : Repository<ShiftDiary, string>, IShiftDiaryRepository
    {
        public ShiftDiaryRepository(SaplsContext context) : base(context)
        {
        }
    }
}
