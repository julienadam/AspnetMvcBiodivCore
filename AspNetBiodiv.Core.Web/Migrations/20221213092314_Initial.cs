using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetBiodiv.Core.Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.CreateTable(
                name: "Especes",
                columns: table => new
                {
                    EspeceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomScientifique = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PresenceEnMetropole = table.Column<int>(type: "int", nullable: false),
                    Habitat = table.Column<int>(type: "int", nullable: false),
                    IdInpn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especes", x => x.EspeceId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            
            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    ObservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObservedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailObservateur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Individus = table.Column<int>(type: "int", nullable: true),
                    NomCommune = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Commentaires = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspeceObserveeEspeceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.ObservationId);
                    table.ForeignKey(
                        name: "FK_Observations_Especes_EspeceObserveeEspeceId",
                        column: x => x.EspeceObserveeEspeceId,
                        principalTable: "Especes",
                        principalColumn: "EspeceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EspeceTag",
                columns: table => new
                {
                    EspecesEspeceId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspeceTag", x => new { x.EspecesEspeceId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_EspeceTag_Especes_EspecesEspeceId",
                        column: x => x.EspecesEspeceId,
                        principalTable: "Especes",
                        principalColumn: "EspeceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EspeceTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

           migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
            
            migrationBuilder.CreateIndex(
                name: "IX_Especes_NomScientifique",
                table: "Especes",
                column: "NomScientifique",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EspeceTag_TagsTagId",
                table: "EspeceTag",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_EspeceObserveeEspeceId",
                table: "Observations",
                column: "EspeceObserveeEspeceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Nom",
                table: "Tags",
                column: "Nom",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EspeceTag");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
            
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Especes");
        }
    }
}
