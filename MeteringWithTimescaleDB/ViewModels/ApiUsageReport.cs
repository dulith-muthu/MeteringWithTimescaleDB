using MeteringTest.Database.Models;

namespace MeteringTest.ViewModels
{
    public class ApiUsageReport
    {
        public string Customer { get; set; } = null!;

        public IReadOnlyCollection<ApiUsageMinutely> Usage { get; set; } = new List<ApiUsageMinutely>();
    }
}
