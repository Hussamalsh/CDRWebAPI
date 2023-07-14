using CDRWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CDRWebAPI.DBContext
{
    public interface ICDRContext : IDisposable
    {
        DbSet<CDR> CDRs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
