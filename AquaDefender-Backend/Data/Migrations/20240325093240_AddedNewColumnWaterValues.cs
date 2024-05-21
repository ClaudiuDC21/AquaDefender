using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnWaterValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotalitateaSolidelorDizolvate",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalitateaSolidelorDizolvate",
                table: "WaterInfos");
        }
    }
}
