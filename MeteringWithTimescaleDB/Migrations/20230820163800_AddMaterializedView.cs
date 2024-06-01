using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteringTest.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterializedView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create materialized views of ApiUsages (this does not work inside migrations scripts/ non-transactional)
            var materializedView = @"
CREATE MATERIALIZED VIEW ""ApiUsageSummaryMinutely""(""Time"", ""ContextIdentifier"", ""CpuTime"") 
WITH (timescaledb.continuous)
AS SELECT
    time_bucket(INTERVAL '1 minute', ""Time""),
    ""ContextIdentifier"",
    AVG(""CpuTime"")
FROM ""ApiUsages""
GROUP BY ""ContextIdentifier"", time_bucket(INTERVAL '1 minute', ""Time"");

CREATE INDEX ix_context_identifier_time_minutely ON ""ApiUsageSummaryMinutely"" (""ContextIdentifier"", ""Time"" DESC);

SELECT add_retention_policy('""ApiUsages""', INTERVAL '1 day');
";
            migrationBuilder.Sql(materializedView, suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "SELECT remove_retention_policy('\"ApiUsages\"', if_exists => TRUE);" +
                "DROP INDEX IF EXISTS _timescaledb_internal.ix_context_identifier_time_minutely;" +
                "DROP MATERIALIZED VIEW IF EXISTS \"ApiUsageSummaryMinutely\";"
            , suppressTransaction: true);
        }
    }
}
