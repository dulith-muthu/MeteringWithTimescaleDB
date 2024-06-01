using MeteringTest.Database;
using MeteringTest.Database.Models;
using MeteringTest.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MeteringTest.Services
{
    public interface IDummyDataService
    {
        Task CreateDummySamplesForCustAsync(CancellationToken cancelationToken);
    }

    public class DummyDataService : IDummyDataService
    {
        public const string cust1 = "Cust1";
        public const string cust2 = "Cust2";
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Dictionary<string, WaveGenerator> _waveGenerators;

        public DummyDataService(IServiceScopeFactory scopeFactory)
        {
            _waveGenerators = new Dictionary<string, WaveGenerator>() {
                {  cust1 , new WaveGenerator(DateTime.Now, TimeSpan.FromMinutes(10), 30, 60) },
                {  cust2 , new WaveGenerator(DateTime.Now, TimeSpan.FromMinutes(8), 150, 200) },
            };
            _scopeFactory = scopeFactory;
        }

        public async Task CreateDummySamplesForCustAsync(CancellationToken cancelationToken)
        {
            var time = DateTime.Now;
            var data1 = new ApiUsage()
            {
                Time = time,
                ContextIdentifier = cust1,
                CpuTime = _waveGenerators[cust1].GenerateSquare(time)
            };
            var data2 = new ApiUsage()
            {
                Time = time,
                ContextIdentifier = cust2,
                CpuTime = _waveGenerators[cust2].GenerateSawtooth(time)
            };
            var sql = GetApiUsageInsertString(data1) + GetApiUsageInsertString(data2);
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MeteringDbContext>();
            try
            {
                await db.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancelationToken);
                //await db.ApiUsages.AddRangeAsync([data1, data2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private static string GetApiUsageInsertString(ApiUsage apiUsage)
        {
            return $@"INSERT INTO ""{nameof(MeteringDbContext.ApiUsages)}""
(""{nameof(ApiUsage.Time)}"", ""{nameof(ApiUsage.ContextIdentifier)}"", ""{nameof(ApiUsage.CpuTime)}"")
VALUES ('{apiUsage.Time}', '{apiUsage.ContextIdentifier}', {apiUsage.CpuTime});
";
        }
    }
}
