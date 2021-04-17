using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OrmPerformanceTests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Code2 = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestEntity_Code",
                table: "TestEntity",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestEntity_Code2",
                table: "TestEntity",
                column: "Code2");

            migrationBuilder.CreateIndex(
                name: "IX_TestEntity_Name",
                table: "TestEntity",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestEntity");
        }
    }
}
