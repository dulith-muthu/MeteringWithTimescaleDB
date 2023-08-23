using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteringTest.Migrations
{
    /// <inheritdoc />
    public partial class ContinousAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var continousAggregate = @"
SELECT add_continuous_aggregate_policy('""ApiUsageSummaryMinutely""',
    start_offset => INTERVAL '3 minutes',
    end_offset => INTERVAL '1 minute',
    schedule_interval => INTERVAL '1 minute');
";
            migrationBuilder.Sql(continousAggregate);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('\"ApiUsageSummaryMinutely\"', if_exists => TRUE);");
        }
    }
}
