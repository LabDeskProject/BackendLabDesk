using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SLA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SlaInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID(N'[sla].[Policies]', N'U') IS NOT NULL OR OBJECT_ID(N'[sla].[Clocks]', N'U') IS NOT NULL THROW 51005, 'SLA migration refuses to overwrite existing data.', 1;");

            migrationBuilder.EnsureSchema(
                name: "sla");

            migrationBuilder.CreateTable(
                name: "Clocks",
                schema: "sla",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDueAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolutionDueAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstResponseAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                schema: "sla",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseMinutes = table.Column<int>(type: "int", nullable: false),
                    ResolutionMinutes = table.Column<int>(type: "int", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clocks_OrganizationId_Status_ResolutionDueAtUtc",
                schema: "sla",
                table: "Clocks",
                columns: new[] { "OrganizationId", "Status", "ResolutionDueAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Clocks_RequestId",
                schema: "sla",
                table: "Clocks",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policies_OrganizationId_ServiceId",
                schema: "sla",
                table: "Policies",
                columns: new[] { "OrganizationId", "ServiceId" },
                unique: true,
                filter: "[IsArchived] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clocks",
                schema: "sla");

            migrationBuilder.DropTable(
                name: "Policies",
                schema: "sla");
        }
    }
}
