using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR4You.Migrations.WorkTime
{
    /// <inheritdoc />
    public partial class InitialWorkTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hr4you_WorkTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MinMonHours = table.Column<int>(type: "int", nullable: false),
                    MinTueHours = table.Column<int>(type: "int", nullable: false),
                    MinWedHours = table.Column<int>(type: "int", nullable: false),
                    MinThuHours = table.Column<int>(type: "int", nullable: false),
                    MinFriHours = table.Column<int>(type: "int", nullable: false),
                    MinSatHours = table.Column<int>(type: "int", nullable: false),
                    MinSunHours = table.Column<int>(type: "int", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hr4you_WorkTime", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hr4you_WorkTime");
        }
    }
}
