using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PawsitiveScheduling.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Name = table.Column<int>(nullable: false),
                    GroomMinutes = table.Column<int>(nullable: false),
                    BathMinutes = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Group = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Name = table.Column<string>(nullable: false),
                    BreedName = table.Column<int>(nullable: true),
                    AdditionalGroomMinutes = table.Column<int>(nullable: false),
                    AdditionalBathMinutes = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dogs_Breeds_BreedName",
                        column: x => x.BreedName,
                        principalTable: "Breeds",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_BreedName",
                table: "Dogs",
                column: "BreedName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "Breeds");
        }
    }
}
