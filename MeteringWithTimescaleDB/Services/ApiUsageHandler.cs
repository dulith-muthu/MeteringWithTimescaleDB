using MeteringTest.Database;
using MeteringTest.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MeteringTest.Services
{
    public interface IApiUsageHandler {
        Task<ApiUsageReport> GetUsage(DateTime from, DateTime to, string cust, CancellationToken cancellationToken);
    }

    public class ApiUsageHandler : IApiUsageHandler
    {
        MeteringDbContext _db;

        public ApiUsageHandler(MeteringDbContext db) {
            _db = db;
        }

        public async Task<ApiUsageReport> GetUsage(DateTime from, DateTime to, string cust, CancellationToken cancellationToken)
        {
            var usages = await _db.ApiUsageSummaryMinutely
                .Where(x => x.ContextIdentifier == cust)
                .Where(x => x.Time < to)
                .Where(x => x.Time > from)
                .OrderByDescending(x => x.Time)
                .ToListAsync();
            return new ApiUsageReport {
                Customer = cust,
                Usage = usages.ToList(),
            };
        }
    }
}
