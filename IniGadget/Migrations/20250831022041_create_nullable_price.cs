using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IniGadget.Migrations
{
    /// <inheritdoc />
    public partial class create_nullable_price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "Stock",
                table: "Products",
                type: "bigint unsigned",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AlterColumn<ulong>(
                name: "Price",
                table: "Products",
                type: "bigint unsigned",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "Stock",
                table: "Products",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");

            migrationBuilder.AlterColumn<uint>(
                name: "Price",
                table: "Products",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");
        }
    }
}
