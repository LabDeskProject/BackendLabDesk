using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServiceCatalogInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID(N'[service_catalog].[Services]', N'U') IS NOT NULL THROW 51001, 'ServiceCatalog initial migration refuses to overwrite existing data.', 1;");

            migrationBuilder.EnsureSchema(
                name: "service_catalog");

            migrationBuilder.CreateTable(
                name: "Services",
                schema: "service_catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrganizationId_Name",
                schema: "service_catalog",
                table: "Services",
                columns: new[] { "OrganizationId", "Name" },
                unique: true,
                filter: "[IsArchived] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services",
                schema: "service_catalog");
        }
    }
}
