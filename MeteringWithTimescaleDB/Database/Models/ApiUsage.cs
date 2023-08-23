using Microsoft.EntityFrameworkCore;

namespace MeteringTest.Database.Models
{
    [Keyless]
    public class ApiUsage
    {
        public DateTimeOffset Time { get; set; }

        public string ContextIdentifier { get; set; } = null!;

        public double CpuTime { get; set; }
    }
}
