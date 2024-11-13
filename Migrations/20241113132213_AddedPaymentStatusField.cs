using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10251759_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentStatusField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Claims");
        }
    }
}
