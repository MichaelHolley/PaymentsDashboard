using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentsDashboard.Migrations
{
    public partial class ReoccuringPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReoccuringPaymentId",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReoccuringPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    StartDate = table.Column<string>(type: "TEXT", nullable: true),
                    EndDate = table.Column<string>(type: "TEXT", nullable: true),
                    ReoccuringType = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReoccuringPayments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ReoccuringPaymentId",
                table: "Tags",
                column: "ReoccuringPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ReoccuringPayments_ReoccuringPaymentId",
                table: "Tags",
                column: "ReoccuringPaymentId",
                principalTable: "ReoccuringPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ReoccuringPayments_ReoccuringPaymentId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "ReoccuringPayments");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ReoccuringPaymentId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ReoccuringPaymentId",
                table: "Tags");
        }
    }
}
