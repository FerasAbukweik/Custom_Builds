using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModification_CustomBuilds_CustomBuildsId",
                table: "CustomBuildModification");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModification_Modifications_ModificationsId",
                table: "CustomBuildModification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomBuildModification",
                table: "CustomBuildModification");

            migrationBuilder.RenameTable(
                name: "CustomBuildModification",
                newName: "CustomBuildModifications");

            migrationBuilder.RenameIndex(
                name: "IX_CustomBuildModification_ModificationsId",
                table: "CustomBuildModifications",
                newName: "IX_CustomBuildModifications_ModificationsId");

            migrationBuilder.AddColumn<string>(
                name: "images",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "CustomBuilds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomBuildModifications",
                table: "CustomBuildModifications",
                columns: new[] { "CustomBuildsId", "ModificationsId" });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomBuilds_CreatorId",
                table: "CustomBuilds",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuildModifications_CustomBuilds_CustomBuildsId",
                table: "CustomBuildModifications",
                column: "CustomBuildsId",
                principalTable: "CustomBuilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuildModifications_Modifications_ModificationsId",
                table: "CustomBuildModifications",
                column: "ModificationsId",
                principalTable: "Modifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuilds_AspNetUsers_CreatorId",
                table: "CustomBuilds",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModifications_CustomBuilds_CustomBuildsId",
                table: "CustomBuildModifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModifications_Modifications_ModificationsId",
                table: "CustomBuildModifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuilds_AspNetUsers_CreatorId",
                table: "CustomBuilds");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_CustomBuilds_CreatorId",
                table: "CustomBuilds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomBuildModifications",
                table: "CustomBuildModifications");

            migrationBuilder.DropColumn(
                name: "images",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CustomBuilds");

            migrationBuilder.RenameTable(
                name: "CustomBuildModifications",
                newName: "CustomBuildModification");

            migrationBuilder.RenameIndex(
                name: "IX_CustomBuildModifications_ModificationsId",
                table: "CustomBuildModification",
                newName: "IX_CustomBuildModification_ModificationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomBuildModification",
                table: "CustomBuildModification",
                columns: new[] { "CustomBuildsId", "ModificationsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuildModification_CustomBuilds_CustomBuildsId",
                table: "CustomBuildModification",
                column: "CustomBuildsId",
                principalTable: "CustomBuilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuildModification_Modifications_ModificationsId",
                table: "CustomBuildModification",
                column: "ModificationsId",
                principalTable: "Modifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
