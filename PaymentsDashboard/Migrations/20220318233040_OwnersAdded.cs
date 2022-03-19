using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentsDashboard.Migrations
{
    public partial class OwnersAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "ReoccuringPayments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Payments",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "ReoccuringPayments");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Payments");
        }
    }
}
