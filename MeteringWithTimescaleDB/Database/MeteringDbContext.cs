using MeteringTest.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MeteringTest.Database
{
    public class MeteringDbContext: DbContext
    {
        public const string ConnectionStringName = "MeteringDb";

        public DbSet<ApiUsage> ApiUsages { get; set; }

        /// <summary>
        /// materialized view
        /// </summary>
        public DbSet<ApiUsageMinutely> ApiUsageSummaryMinutely { get; set; }

        public MeteringDbContext(DbContextOptions<MeteringDbContext> options) : base(options) { }
    }
}
