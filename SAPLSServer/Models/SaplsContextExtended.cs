using Microsoft.EntityFrameworkCore;
using SAPLSServer.Constants;

namespace SAPLSServer.Models
{
    public partial class SaplsContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasQueryFilter(u => u.Status != UserStatus.Deleted.ToString());
            modelBuilder.Entity<Vehicle>()
            .HasQueryFilter(u => u.Status != "Deleted");
        }
    }
}
