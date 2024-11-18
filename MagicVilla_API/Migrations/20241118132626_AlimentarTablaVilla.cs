using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualización", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8168), new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8111), "", 5.0, "Villa Real", 5, 200.0 },
                    { 2, "", "Best Luxury Villa", new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8174), new DateTime(2024, 11, 18, 14, 26, 26, 182, DateTimeKind.Local).AddTicks(8172), "", 20.0, "Villa Real Luxury", 10, 400.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
