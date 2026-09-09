using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeuselessfieldfromChatGroupentityandlinkapplicationUserwithImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroup_User_ManyToMany");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_UserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "ChatGroup_User_ManyToMany",
                columns: table => new
                {
                    ChatGroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroup_User_ManyToMany", x => new { x.ChatGroupsId, x.SupportersId });
                    table.ForeignKey(
                        name: "FK_ChatGroup_User_ManyToMany_AspNetUsers_SupportersId",
                        column: x => x.SupportersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatGroup_User_ManyToMany_ChatGroups_ChatGroupsId",
                        column: x => x.ChatGroupsId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroup_User_ManyToMany_SupportersId",
                table: "ChatGroup_User_ManyToMany",
                column: "SupportersId");
        }
    }
}
