using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10251759_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Claims",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Claims");
        }
    }
}
