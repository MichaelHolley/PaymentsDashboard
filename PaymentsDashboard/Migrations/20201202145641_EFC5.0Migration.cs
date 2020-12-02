using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentsDashboard.Migrations
{
    public partial class EFC50Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTagRelations");

            migrationBuilder.CreateTable(
                name: "PaymentTag",
                columns: table => new
                {
                    PaymentsPaymentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TagsTagId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTag", x => new { x.PaymentsPaymentId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_PaymentTag_Payments_PaymentsPaymentId",
                        column: x => x.PaymentsPaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTag_TagsTagId",
                table: "PaymentTag",
                column: "TagsTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTag");

            migrationBuilder.CreateTable(
                name: "PaymentTagRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    PaymentId = table.Column<Guid>(type: "char(36)", nullable: true),
                    TagId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTagRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTagRelations_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTagRelations_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTagRelations_PaymentId",
                table: "PaymentTagRelations",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTagRelations_TagId",
                table: "PaymentTagRelations",
                column: "TagId");
        }
    }
}
