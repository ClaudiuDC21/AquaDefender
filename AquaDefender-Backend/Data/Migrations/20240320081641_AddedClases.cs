using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedClases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "County",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterDeptEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_County", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityHallEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_County_CountyId",
                        column: x => x.CountyId,
                        principalTable: "County",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_County_CountyId",
                        column: x => x.CountyId,
                        principalTable: "County",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WaterInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DateReported = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EscherichiaColi = table.Column<float>(type: "real", nullable: false),
                    Enterococi = table.Column<float>(type: "real", nullable: false),
                    PseudomonasAeruginosa = table.Column<float>(type: "real", nullable: false),
                    NumarColonii22C = table.Column<float>(type: "real", nullable: false),
                    NumarColonii37C = table.Column<float>(type: "real", nullable: false),
                    Acrilamida = table.Column<float>(type: "real", nullable: false),
                    Arsen = table.Column<float>(type: "real", nullable: false),
                    Benzen = table.Column<float>(type: "real", nullable: false),
                    BenzAPiren = table.Column<float>(type: "real", nullable: false),
                    Bor = table.Column<float>(type: "real", nullable: false),
                    Bromati = table.Column<float>(type: "real", nullable: false),
                    Cadmiu = table.Column<float>(type: "real", nullable: false),
                    CloruraDeVinil = table.Column<float>(type: "real", nullable: false),
                    CianuriTotale = table.Column<float>(type: "real", nullable: false),
                    CianuriLibere = table.Column<float>(type: "real", nullable: false),
                    CromTotal = table.Column<float>(type: "real", nullable: false),
                    Cupru = table.Column<float>(type: "real", nullable: false),
                    Dicloretan = table.Column<float>(type: "real", nullable: false),
                    Epiclorhidrina = table.Column<float>(type: "real", nullable: false),
                    Fluoruri = table.Column<float>(type: "real", nullable: false),
                    HidrocarburiPolicicliceAromatice = table.Column<float>(type: "real", nullable: false),
                    Mercur = table.Column<float>(type: "real", nullable: false),
                    Nichel = table.Column<float>(type: "real", nullable: false),
                    Nitrati = table.Column<float>(type: "real", nullable: false),
                    Nitriti = table.Column<float>(type: "real", nullable: false),
                    Pesticide = table.Column<float>(type: "real", nullable: false),
                    PesticideTotal = table.Column<float>(type: "real", nullable: false),
                    Plumb = table.Column<float>(type: "real", nullable: false),
                    Seleniu = table.Column<float>(type: "real", nullable: false),
                    Stibiu = table.Column<float>(type: "real", nullable: false),
                    TetracloretenaSiTricloretena = table.Column<float>(type: "real", nullable: false),
                    TrihalometaniTotal = table.Column<float>(type: "real", nullable: false),
                    Aluminiu = table.Column<float>(type: "real", nullable: false),
                    Amoniu = table.Column<float>(type: "real", nullable: false),
                    BacteriiColiforme = table.Column<float>(type: "real", nullable: false),
                    CarbonOrganicTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cloruri = table.Column<float>(type: "real", nullable: false),
                    ClostridiumPerfringens = table.Column<float>(type: "real", nullable: false),
                    ClorRezidualLiber = table.Column<float>(type: "real", nullable: false),
                    Conductivitate = table.Column<float>(type: "real", nullable: false),
                    Culoare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuritateTotala = table.Column<float>(type: "real", nullable: false),
                    Fier = table.Column<float>(type: "real", nullable: false),
                    Gust = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mangan = table.Column<float>(type: "real", nullable: false),
                    Miros = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Oxidabilitate = table.Column<float>(type: "real", nullable: false),
                    PH = table.Column<float>(type: "real", nullable: false),
                    Sodiu = table.Column<float>(type: "real", nullable: false),
                    Sulfat = table.Column<float>(type: "real", nullable: false),
                    SulfuriSiHidrogenSulfurat = table.Column<float>(type: "real", nullable: false),
                    Turbiditate = table.Column<float>(type: "real", nullable: false),
                    Zinc = table.Column<float>(type: "real", nullable: false),
                    Tritiu = table.Column<float>(type: "real", nullable: false),
                    DozaEfectivaTotalaDeReferinta = table.Column<float>(type: "real", nullable: false),
                    ActivitateaAlfaGlobala = table.Column<float>(type: "real", nullable: false),
                    ActivitateaBetaGlobala = table.Column<float>(type: "real", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInfos_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaterInfos_County_CountyId",
                        column: x => x.CountyId,
                        principalTable: "County",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    LocationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_County_CountyId",
                        column: x => x.CountyId,
                        principalTable: "County",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportImage",
                columns: table => new
                {
                    IdImage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportImage", x => x.IdImage);
                    table.ForeignKey(
                        name: "FK_ReportImage_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CountyId",
                table: "City",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportImage_ReportId",
                table: "ReportImage",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CityId",
                table: "Reports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CountyId",
                table: "Reports",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountyId",
                table: "Users",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInfos_CityId",
                table: "WaterInfos",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInfos_CountyId",
                table: "WaterInfos",
                column: "CountyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportImage");

            migrationBuilder.DropTable(
                name: "WaterInfos");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "County");
        }
    }
}
