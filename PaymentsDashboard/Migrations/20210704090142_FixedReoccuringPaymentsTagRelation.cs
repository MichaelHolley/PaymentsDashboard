using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentsDashboard.Migrations
{
    public partial class FixedReoccuringPaymentsTagRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ReoccuringPayments_ReoccuringPaymentId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ReoccuringPaymentId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ReoccuringPaymentId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ReoccuringPaymentTag",
                columns: table => new
                {
                    ReoccuringPaymentsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagsTagId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReoccuringPaymentTag", x => new { x.ReoccuringPaymentsId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_ReoccuringPaymentTag_ReoccuringPayments_ReoccuringPaymentsId",
                        column: x => x.ReoccuringPaymentsId,
                        principalTable: "ReoccuringPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReoccuringPaymentTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReoccuringPaymentTag_TagsTagId",
                table: "ReoccuringPaymentTag",
                column: "TagsTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReoccuringPaymentTag");

            migrationBuilder.AddColumn<Guid>(
                name: "ReoccuringPaymentId",
                table: "Tags",
                type: "TEXT",
                nullable: true);

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
    }
}
