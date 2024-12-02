using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR4You.Migrations.HourEntry
{
    /// <inheritdoc />
    public partial class AddedHolidayAndBillFlagToHourEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBillable",
                table: "hr4you_HourEntry",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "hr4you_HourEntry",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBillable",
                table: "hr4you_HourEntry");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "hr4you_HourEntry");
        }
    }
}
