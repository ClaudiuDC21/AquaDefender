using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllValuesIntoListOfValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acrilamida",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "ActivitateaAlfaGlobala",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "ActivitateaBetaGlobala",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Aluminiu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Amoniu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Arsen",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "BacteriiColiforme",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "BenzAPiren",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Benzen",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Bor",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Bromati",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Cadmiu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "CarbonOrganicTotal",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "CianuriLibere",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "CianuriTotale",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "ClorRezidualLiber",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "CloruraDeVinil",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Cloruri",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "ClostridiumPerfringens",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Conductivitate",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "CromTotal",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Culoare",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Cupru",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Dicloretan",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "DozaEfectivaTotalaDeReferinta",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "DuritateTotala",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Enterococi",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Epiclorhidrina",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "EscherichiaColi",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Fier",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Fluoruri",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Gust",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "HidrocarburiPolicicliceAromatice",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Mangan",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Mercur",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Miros",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Nichel",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Nitrati",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Nitriti",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "NumarColonii22C",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "NumarColonii37C",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Oxidabilitate",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "PH",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Pesticide",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "PesticideTotal",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Plumb",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "PseudomonasAeruginosa",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Seleniu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Sodiu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Stibiu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Sulfat",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "SulfuriSiHidrogenSulfurat",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "TetracloretenaSiTricloretena",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "TotalitateaSolidelorDizolvate",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "TrihalometaniTotal",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Tritiu",
                table: "WaterInfos");

            migrationBuilder.DropColumn(
                name: "Turbiditate",
                table: "WaterInfos");

            migrationBuilder.RenameColumn(
                name: "Zinc",
                table: "WaterInfos",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "WaterValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumAllowedValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProvidedValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdWaterInfo = table.Column<int>(type: "int", nullable: false),
                    WaterInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterValues_WaterInfos_WaterInfoId",
                        column: x => x.WaterInfoId,
                        principalTable: "WaterInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterValues_WaterInfoId",
                table: "WaterValues",
                column: "WaterInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterValues");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WaterInfos",
                newName: "Zinc");

            migrationBuilder.AddColumn<string>(
                name: "Acrilamida",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivitateaAlfaGlobala",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivitateaBetaGlobala",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Aluminiu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Amoniu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Arsen",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BacteriiColiforme",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenzAPiren",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Benzen",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bor",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bromati",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cadmiu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarbonOrganicTotal",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CianuriLibere",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CianuriTotale",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClorRezidualLiber",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CloruraDeVinil",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cloruri",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClostridiumPerfringens",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conductivitate",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CromTotal",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Culoare",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cupru",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dicloretan",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DozaEfectivaTotalaDeReferinta",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DuritateTotala",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Enterococi",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Epiclorhidrina",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EscherichiaColi",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fier",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fluoruri",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gust",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HidrocarburiPolicicliceAromatice",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mangan",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mercur",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Miros",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nichel",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nitrati",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nitriti",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumarColonii22C",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumarColonii37C",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Oxidabilitate",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PH",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pesticide",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PesticideTotal",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plumb",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PseudomonasAeruginosa",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seleniu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sodiu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stibiu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sulfat",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SulfuriSiHidrogenSulfurat",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TetracloretenaSiTricloretena",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalitateaSolidelorDizolvate",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrihalometaniTotal",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tritiu",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Turbiditate",
                table: "WaterInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
