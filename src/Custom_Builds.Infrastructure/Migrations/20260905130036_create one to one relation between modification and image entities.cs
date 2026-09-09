using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createonetoonerelationbetweenmodificationandimageentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Modifications");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Modifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Modifications_ImageId",
                table: "Modifications",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modifications_Images_ImageId",
                table: "Modifications",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifications_Images_ImageId",
                table: "Modifications");

            migrationBuilder.DropIndex(
                name: "IX_Modifications_ImageId",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Modifications");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Modifications",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");
        }
    }
}
