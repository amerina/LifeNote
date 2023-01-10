using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motto.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "motto_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "motto_language_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "motto_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "MottoLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MottoLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MottoType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MottoType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MottoTypeId = table.Column<int>(type: "int", nullable: false),
                    MottoLanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motto_MottoLanguage_MottoLanguageId",
                        column: x => x.MottoLanguageId,
                        principalTable: "MottoLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Motto_MottoType_MottoTypeId",
                        column: x => x.MottoTypeId,
                        principalTable: "MottoType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motto_MottoLanguageId",
                table: "Motto",
                column: "MottoLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Motto_MottoTypeId",
                table: "Motto",
                column: "MottoTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motto");

            migrationBuilder.DropTable(
                name: "MottoLanguage");

            migrationBuilder.DropTable(
                name: "MottoType");

            migrationBuilder.DropSequence(
                name: "motto_hilo");

            migrationBuilder.DropSequence(
                name: "motto_language_hilo");

            migrationBuilder.DropSequence(
                name: "motto_type_hilo");
        }
    }
}
