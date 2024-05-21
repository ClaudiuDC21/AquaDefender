using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class addCascadeOnReportImagesDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportImage_Reports_ReportId",
                table: "ReportImage");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportImage_Reports_ReportId",
                table: "ReportImage",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportImage_Reports_ReportId",
                table: "ReportImage");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportImage_Reports_ReportId",
                table: "ReportImage",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");
        }
    }
}
