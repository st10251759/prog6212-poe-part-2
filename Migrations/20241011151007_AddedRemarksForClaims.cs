using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10251759_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddedRemarksForClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoordinatorRemarks",
                table: "Claims",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerRemarks",
                table: "Claims",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatorRemarks",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ManagerRemarks",
                table: "Claims");
        }
    }
}
