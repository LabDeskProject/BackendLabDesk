using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationMembershipLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAtUtc",
                schema: "organization_management",
                table: "OrganizationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUsers_OrganizationId_RemovedAtUtc",
                schema: "organization_management",
                table: "OrganizationUsers",
                columns: new[] { "OrganizationId", "RemovedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationUsers_OrganizationId_RemovedAtUtc",
                schema: "organization_management",
                table: "OrganizationUsers");

            migrationBuilder.DropColumn(
                name: "RemovedAtUtc",
                schema: "organization_management",
                table: "OrganizationUsers");
        }
    }
}
