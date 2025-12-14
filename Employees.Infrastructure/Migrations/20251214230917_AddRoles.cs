using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Employees.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Registrations",
                table: "EmployeeRoles",
                columns: new[] { "Id", "Description", "Level" },
                values: new object[,]
                {
                    { 1, "Employee", 1 },
                    { 2, "Leader", 2 },
                    { 3, "Director", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Registrations",
                table: "EmployeeRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Registrations",
                table: "EmployeeRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Registrations",
                table: "EmployeeRoles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
