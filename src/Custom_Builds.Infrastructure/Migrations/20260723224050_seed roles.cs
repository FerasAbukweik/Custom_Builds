using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7c9e31d4-82f5-4e1b-a639-2d14e08f5193"), "4b8f19e2-36c7-4d9a-8b15-20e8d3fa91a4", "Admin", "ADMIN" },
                    { new Guid("a1d7f40e-5c82-411a-96e3-2b8f9e01d43c"), "e82c19a4-67d1-4b3f-b982-14d20f5a89e6", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c9e31d4-82f5-4e1b-a639-2d14e08f5193"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a1d7f40e-5c82-411a-96e3-2b8f9e01d43c"));
        }
    }
}
