using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualización", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 11, 20, 12, 30, 37, 395, DateTimeKind.Local).AddTicks(7914), new DateTime(2024, 11, 20, 12, 30, 37, 395, DateTimeKind.Local).AddTicks(7834) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualización", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 11, 20, 12, 30, 37, 395, DateTimeKind.Local).AddTicks(7922), new DateTime(2024, 11, 20, 12, 30, 37, 395, DateTimeKind.Local).AddTicks(7920) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualización", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8168), new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8111) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualización", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8174), new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8172) });
        }
    }
}
