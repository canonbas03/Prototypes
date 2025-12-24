using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelMVCPrototype.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "ServiceRequestItems");

            migrationBuilder.AddColumn<int>(
                name: "RequestItemId",
                table: "ServiceRequestItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RequestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestItems_RequestItemId",
                table: "ServiceRequestItems",
                column: "RequestItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestItems_RequestItems_RequestItemId",
                table: "ServiceRequestItems",
                column: "RequestItemId",
                principalTable: "RequestItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestItems_RequestItems_RequestItemId",
                table: "ServiceRequestItems");

            migrationBuilder.DropTable(
                name: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestItems_RequestItemId",
                table: "ServiceRequestItems");

            migrationBuilder.DropColumn(
                name: "RequestItemId",
                table: "ServiceRequestItems");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "ServiceRequestItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
