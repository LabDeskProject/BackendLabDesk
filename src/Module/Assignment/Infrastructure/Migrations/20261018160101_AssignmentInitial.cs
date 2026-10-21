using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID(N'[assignment].[Assignments]', N'U') IS NOT NULL THROW 51004, 'Assignment migration refuses to overwrite existing data.', 1;");

            migrationBuilder.EnsureSchema(
                name: "assignment");

            migrationBuilder.CreateTable(
                name: "Assignments",
                schema: "assignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_OrganizationId_AssignedToUserId_Status_AssignedAtUtc",
                schema: "assignment",
                table: "Assignments",
                columns: new[] { "OrganizationId", "AssignedToUserId", "Status", "AssignedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RequestId",
                schema: "assignment",
                table: "Assignments",
                column: "RequestId",
                unique: true,
                filter: "[Status] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments",
                schema: "assignment");
        }
    }
}
