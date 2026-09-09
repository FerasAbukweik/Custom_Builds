using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedescriptionandiconfrommodificationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Modifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modifications",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Modifications",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
