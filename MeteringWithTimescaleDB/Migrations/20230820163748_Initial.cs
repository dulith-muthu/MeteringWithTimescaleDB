using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteringTest.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsages",
                columns: table => new
                {
                    Time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContextIdentifier = table.Column<string>(type: "text", nullable: false),
                    CpuTime = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                });
            // Convert ApiUsages Table to Hypertable
            // language=sql
            migrationBuilder.Sql(
                "SELECT create_hypertable( '\"ApiUsages\"', 'Time');\n" +
                "CREATE INDEX ix_context_identifier_time ON \"ApiUsages\" (\"ContextIdentifier\", \"Time\" DESC);"
            );

            // materialized view is added in next migration
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //hypertable is deleted automatically
            migrationBuilder.DropTable(
                name: "ApiUsages");
            migrationBuilder.Sql(
                "DROP INDEX IF EXISTS ix_context_identifier_time"
            );
        }
    }
}
