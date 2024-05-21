using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class WaterValuesInModelBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterValues_WaterInfos_WaterInfoId",
                table: "WaterValues");

            migrationBuilder.DropIndex(
                name: "IX_WaterValues_WaterInfoId",
                table: "WaterValues");

            migrationBuilder.DropColumn(
                name: "WaterInfoId",
                table: "WaterValues");

            migrationBuilder.CreateIndex(
                name: "IX_WaterValues_IdWaterInfo",
                table: "WaterValues",
                column: "IdWaterInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues",
                column: "IdWaterInfo",
                principalTable: "WaterInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterValues_WaterInfos_IdWaterInfo",
                table: "WaterValues");

            migrationBuilder.DropIndex(
                name: "IX_WaterValues_IdWaterInfo",
                table: "WaterValues");

            migrationBuilder.AddColumn<int>(
                name: "WaterInfoId",
                table: "WaterValues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterValues_WaterInfoId",
                table: "WaterValues",
                column: "WaterInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterValues_WaterInfos_WaterInfoId",
                table: "WaterValues",
                column: "WaterInfoId",
                principalTable: "WaterInfos",
                principalColumn: "Id");
        }
    }
}
