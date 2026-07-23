using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Custom_Builds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class somechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModifications_CustomBuilds_CustomBuildsId",
                table: "CustomBuildModifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuildModifications_Modifications_ModificationsId",
                table: "CustomBuildModifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Modifications_Sections_SectionId",
                table: "Modifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Parts_PartId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_PartId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Modifications_SectionId",
                table: "Modifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomBuildModifications",
                table: "CustomBuildModifications");

            migrationBuilder.DropColumn(
                name: "PartId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "CustomBuildModifications",
                newName: "CustomBuilds_Modifications_ManyToMany");

            migrationBuilder.RenameIndex(
                name: "IX_CustomBuildModifications_ModificationsId",
                table: "CustomBuilds_Modifications_ManyToMany",
                newName: "IX_CustomBuilds_Modifications_ManyToMany_ModificationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomBuilds_Modifications_ManyToMany",
                table: "CustomBuilds_Modifications_ManyToMany",
                columns: new[] { "CustomBuildsId", "ModificationsId" });

            migrationBuilder.CreateTable(
                name: "Section_Parts_ManyToMany",
                columns: table => new
                {
                    PartsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section_Parts_ManyToMany", x => new { x.PartsId, x.SectionsId });
                    table.ForeignKey(
                        name: "FK_Section_Parts_ManyToMany_Parts_PartsId",
                        column: x => x.PartsId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Section_Parts_ManyToMany_Sections_SectionsId",
                        column: x => x.SectionsId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections_Modifications_ManyToMany",
                columns: table => new
                {
                    ModificationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections_Modifications_ManyToMany", x => new { x.ModificationsId, x.SectionsId });
                    table.ForeignKey(
                        name: "FK_Sections_Modifications_ManyToMany_Modifications_ModificationsId",
                        column: x => x.ModificationsId,
                        principalTable: "Modifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Modifications_ManyToMany_Sections_SectionsId",
                        column: x => x.SectionsId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_RefreshTokenString",
                table: "RefreshTokens",
                column: "RefreshTokenString",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Section_Parts_ManyToMany_SectionsId",
                table: "Section_Parts_ManyToMany",
                column: "SectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Modifications_ManyToMany_SectionsId",
                table: "Sections_Modifications_ManyToMany",
                column: "SectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuilds_Modifications_ManyToMany_CustomBuilds_CustomBuildsId",
                table: "CustomBuilds_Modifications_ManyToMany",
                column: "CustomBuildsId",
                principalTable: "CustomBuilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomBuilds_Modifications_ManyToMany_Modifications_ModificationsId",
                table: "CustomBuilds_Modifications_ManyToMany",
                column: "ModificationsId",
                principalTable: "Modifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuilds_Modifications_ManyToMany_CustomBuilds_CustomBuildsId",
                table: "CustomBuilds_Modifications_ManyToMany");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomBuilds_Modifications_ManyToMany_Modifications_ModificationsId",
                table: "CustomBuilds_Modifications_ManyToMany");

            migrationBuilder.DropTable(
                name: "Section_Parts_ManyToMany");

            migrationBuilder.DropTable(
                name: "Sections_Modifications_ManyToMany");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_RefreshTokenString",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomBuilds_Modifications_ManyToMany",
                table: "CustomBuilds_Modifications_ManyToMany");

            migrationBuilder.RenameTable(
                name: "CustomBuilds_Modifications_ManyToMany",
                newName: "CustomBuildModifications");

            migrationBuilder.RenameIndex(
                name: "IX_CustomBuilds_Modifications_ManyToMany_ModificationsId",
                table: "CustomBuildModifications",
                newName: "IX_CustomBuildModifications_ModificationsId");

            migrationBuilder.AddColumn<Guid>(
                name: "PartId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "Modifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomBuildModifications",
                table: "CustomBuildModifications",
                columns: new[] { "CustomBuildsId", "ModificationsId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PartId",
                table: "Sections",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifications_SectionId",
                table: "Modifications",
                column: "SectionId");

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
                name: "FK_Modifications_Sections_SectionId",
                table: "Modifications",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Parts_PartId",
                table: "Sections",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
