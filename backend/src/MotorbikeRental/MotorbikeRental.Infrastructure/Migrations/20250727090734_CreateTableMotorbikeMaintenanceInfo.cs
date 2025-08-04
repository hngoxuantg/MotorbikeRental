using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorbikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableMotorbikeMaintenanceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "MaintenanceRecord");

            migrationBuilder.CreateTable(
                name: "MotorbikeMaintenanceInfo",
                columns: table => new
                {
                    MotorbikeId = table.Column<int>(type: "int", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorbikeMaintenanceInfo", x => x.MotorbikeId);
                    table.ForeignKey(
                        name: "FK_MotorbikeMaintenanceInfo_Motorbike_MotorbikeId",
                        column: x => x.MotorbikeId,
                        principalTable: "Motorbike",
                        principalColumn: "MotorbikeId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorbikeMaintenanceInfo");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "MaintenanceRecord",
                type: "datetime2",
                nullable: true);
        }
    }
}
