using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelMVCPrototype.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomMapPercentages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapHeight",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapLeft",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapTop",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapWidth",
                table: "Rooms");

            migrationBuilder.AddColumn<double>(
                name: "MapHeightPercent",
                table: "Rooms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MapLeftPercent",
                table: "Rooms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MapTopPercent",
                table: "Rooms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MapWidthPercent",
                table: "Rooms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapHeightPercent",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapLeftPercent",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapTopPercent",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MapWidthPercent",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "MapHeight",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MapLeft",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MapTop",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MapWidth",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
