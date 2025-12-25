using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelMVCPrototype.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxQuantityToRequestItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxQuantity",
                table: "RequestItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxQuantity",
                table: "RequestItems");
        }
    }
}
