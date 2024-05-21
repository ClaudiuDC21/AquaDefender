using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedIntoDeleteOnCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues",
                column: "IdWaterInfo",
                principalTable: "WaterInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues",
                column: "IdWaterInfo",
                principalTable: "WaterInfos",
                principalColumn: "Id");
        }
    }
}
