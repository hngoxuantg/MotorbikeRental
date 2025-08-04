using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablePayment_RentalContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Discount_DiscountId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_DiscountId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Payment");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "RentalContract",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "RentalContract",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalContract_DiscountId",
                table: "RentalContract",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContract_Discount_DiscountId",
                table: "RentalContract",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalContract_Discount_DiscountId",
                table: "RentalContract");

            migrationBuilder.DropIndex(
                name: "IX_RentalContract_DiscountId",
                table: "RentalContract");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "RentalContract");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "RentalContract");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_DiscountId",
                table: "Payment",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Discount_DiscountId",
                table: "Payment",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
