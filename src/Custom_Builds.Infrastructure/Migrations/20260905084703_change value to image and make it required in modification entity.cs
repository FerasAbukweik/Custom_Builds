using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changevaluetoimageandmakeitrequiredinmodificationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Modifications");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Modifications",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Modifications");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Modifications",
                type: "varchar(150)",
                nullable: true);
        }
    }
}
