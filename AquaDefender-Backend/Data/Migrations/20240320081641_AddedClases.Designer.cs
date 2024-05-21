﻿// <auto-generated />
using System;
using AquaDefender_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AquaDefender_Backend.Data.Migrations
{
    [DbContext(typeof(AquaDefenderDataContext))]
    [Migration("20240320081641_AddedClases")]
    partial class AddedClases
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AquaDefender_Backend.Domain.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("CountyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountyId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CityHallEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountyId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.County", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WaterDeptEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("County");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("CountyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("LocationDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Severity")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountyId");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.ReportImage", b =>
                {
                    b.Property<int>("IdImage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdImage"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("IdImage");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportImage");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.WaterInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Acrilamida")
                        .HasColumnType("real");

                    b.Property<float>("ActivitateaAlfaGlobala")
                        .HasColumnType("real");

                    b.Property<float>("ActivitateaBetaGlobala")
                        .HasColumnType("real");

                    b.Property<string>("AdditionalNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Aluminiu")
                        .HasColumnType("real");

                    b.Property<float>("Amoniu")
                        .HasColumnType("real");

                    b.Property<float>("Arsen")
                        .HasColumnType("real");

                    b.Property<float>("BacteriiColiforme")
                        .HasColumnType("real");

                    b.Property<float>("BenzAPiren")
                        .HasColumnType("real");

                    b.Property<float>("Benzen")
                        .HasColumnType("real");

                    b.Property<float>("Bor")
                        .HasColumnType("real");

                    b.Property<float>("Bromati")
                        .HasColumnType("real");

                    b.Property<float>("Cadmiu")
                        .HasColumnType("real");

                    b.Property<string>("CarbonOrganicTotal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("CianuriLibere")
                        .HasColumnType("real");

                    b.Property<float>("CianuriTotale")
                        .HasColumnType("real");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<float>("ClorRezidualLiber")
                        .HasColumnType("real");

                    b.Property<float>("CloruraDeVinil")
                        .HasColumnType("real");

                    b.Property<float>("Cloruri")
                        .HasColumnType("real");

                    b.Property<float>("ClostridiumPerfringens")
                        .HasColumnType("real");

                    b.Property<float>("Conductivitate")
                        .HasColumnType("real");

                    b.Property<int>("CountyId")
                        .HasColumnType("int");

                    b.Property<float>("CromTotal")
                        .HasColumnType("real");

                    b.Property<string>("Culoare")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Cupru")
                        .HasColumnType("real");

                    b.Property<DateTime>("DateReported")
                        .HasColumnType("datetime2");

                    b.Property<float>("Dicloretan")
                        .HasColumnType("real");

                    b.Property<float>("DozaEfectivaTotalaDeReferinta")
                        .HasColumnType("real");

                    b.Property<float>("DuritateTotala")
                        .HasColumnType("real");

                    b.Property<float>("Enterococi")
                        .HasColumnType("real");

                    b.Property<float>("Epiclorhidrina")
                        .HasColumnType("real");

                    b.Property<float>("EscherichiaColi")
                        .HasColumnType("real");

                    b.Property<float>("Fier")
                        .HasColumnType("real");

                    b.Property<float>("Fluoruri")
                        .HasColumnType("real");

                    b.Property<string>("Gust")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("HidrocarburiPolicicliceAromatice")
                        .HasColumnType("real");

                    b.Property<float>("Mangan")
                        .HasColumnType("real");

                    b.Property<float>("Mercur")
                        .HasColumnType("real");

                    b.Property<string>("Miros")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Nichel")
                        .HasColumnType("real");

                    b.Property<float>("Nitrati")
                        .HasColumnType("real");

                    b.Property<float>("Nitriti")
                        .HasColumnType("real");

                    b.Property<float>("NumarColonii22C")
                        .HasColumnType("real");

                    b.Property<float>("NumarColonii37C")
                        .HasColumnType("real");

                    b.Property<float>("Oxidabilitate")
                        .HasColumnType("real");

                    b.Property<float>("PH")
                        .HasColumnType("real");

                    b.Property<float>("Pesticide")
                        .HasColumnType("real");

                    b.Property<float>("PesticideTotal")
                        .HasColumnType("real");

                    b.Property<float>("Plumb")
                        .HasColumnType("real");

                    b.Property<float>("PseudomonasAeruginosa")
                        .HasColumnType("real");

                    b.Property<float>("Seleniu")
                        .HasColumnType("real");

                    b.Property<float>("Sodiu")
                        .HasColumnType("real");

                    b.Property<float>("Stibiu")
                        .HasColumnType("real");

                    b.Property<float>("Sulfat")
                        .HasColumnType("real");

                    b.Property<float>("SulfuriSiHidrogenSulfurat")
                        .HasColumnType("real");

                    b.Property<float>("TetracloretenaSiTricloretena")
                        .HasColumnType("real");

                    b.Property<float>("TrihalometaniTotal")
                        .HasColumnType("real");

                    b.Property<float>("Tritiu")
                        .HasColumnType("real");

                    b.Property<float>("Turbiditate")
                        .HasColumnType("real");

                    b.Property<float>("Zinc")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountyId");

                    b.ToTable("WaterInfos");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.AppUser", b =>
                {
                    b.HasOne("AquaDefender_Backend.Domain.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AquaDefender_Backend.Domain.County", "County")
                        .WithMany()
                        .HasForeignKey("CountyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AquaDefender_Backend.Domain.UserRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("County");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.City", b =>
                {
                    b.HasOne("AquaDefender_Backend.Domain.County", "County")
                        .WithMany("Cities")
                        .HasForeignKey("CountyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("County");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.Report", b =>
                {
                    b.HasOne("AquaDefender_Backend.Domain.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AquaDefender_Backend.Domain.County", "County")
                        .WithMany()
                        .HasForeignKey("CountyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AquaDefender_Backend.Domain.AppUser", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("County");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.ReportImage", b =>
                {
                    b.HasOne("AquaDefender_Backend.Domain.Report", "Report")
                        .WithMany("Images")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Report");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.WaterInfo", b =>
                {
                    b.HasOne("AquaDefender_Backend.Domain.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AquaDefender_Backend.Domain.County", "County")
                        .WithMany()
                        .HasForeignKey("CountyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("County");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.AppUser", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.County", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("AquaDefender_Backend.Domain.Report", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
