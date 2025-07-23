using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wpf_Plc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlcModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    DigitalInputsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DigitalOutputsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AnalogInputsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AnalogOutputsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SupportsExpansionModules = table.Column<bool>(type: "INTEGER", nullable: false),
                    PowerSupplyType = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerConsumption = table.Column<double>(type: "REAL", nullable: false),
                    SupportsEthernet = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsRS232 = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsRS485 = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsUsb = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsCanBus = table.Column<bool>(type: "INTEGER", nullable: false),
                    ManufacturerURL = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlcModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlcDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IPAddressString = table.Column<string>(type: "TEXT", nullable: false),
                    PortName = table.Column<string>(type: "TEXT", nullable: false),
                    BaudRate = table.Column<int>(type: "INTEGER", nullable: false),
                    DataBits = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlcDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlcDevices_PlcModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "PlcModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlcPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalFileName = table.Column<string>(type: "TEXT", nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    FileExtension = table.Column<string>(type: "TEXT", nullable: false),
                    PLCModelId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlcPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlcPrograms_PlcModels_PLCModelId1",
                        column: x => x.PLCModelId1,
                        principalTable: "PlcModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlcDevices_ModelId",
                table: "PlcDevices",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PlcPrograms_PLCModelId1",
                table: "PlcPrograms",
                column: "PLCModelId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlcDevices");

            migrationBuilder.DropTable(
                name: "PlcPrograms");

            migrationBuilder.DropTable(
                name: "PlcModels");
        }
    }
}
