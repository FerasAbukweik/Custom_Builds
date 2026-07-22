using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addchatgroupentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatGroupId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

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
                name: "IX_Messages_ChatGroupId",
                table: "Messages",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroup_User_ManyToMany_SupportersId",
                table: "ChatGroup_User_ManyToMany",
                column: "SupportersId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_UserId",
                table: "ChatGroups",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatGroups_ChatGroupId",
                table: "Messages",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatGroups_ChatGroupId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatGroup_User_ManyToMany");

            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatGroupId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatGroupId",
                table: "Messages");
        }
    }
}
