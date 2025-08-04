using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Incident_ContractId",
                table: "Incident");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Incident",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_ContractId",
                table: "Incident",
                column: "ContractId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Incident_ContractId",
                table: "Incident");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Incident",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_ContractId",
                table: "Incident",
                column: "ContractId",
                unique: true,
                filter: "[ContractId] IS NOT NULL");
        }
    }
}
