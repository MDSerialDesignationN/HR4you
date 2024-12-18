using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR4You.Migrations.WorkTime
{
    /// <inheritdoc />
    public partial class RemovedHolidaysWorkTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Holidays",
                table: "hr4you_worktime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Holidays",
                table: "hr4you_worktime",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
