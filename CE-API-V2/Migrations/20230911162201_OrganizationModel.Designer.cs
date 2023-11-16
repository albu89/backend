﻿// <auto-generated />
using System;
using CE_API_V2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CE_API_V2.Migrations
{
    [DbContext(typeof(CEContext))]
    [Migration("20230911162201_OrganizationModel")]
    partial class OrganizationModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CE_API_V2.Models.BiomarkerOrderModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BiomarkerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("int");

                    b.Property<string>("PreferredUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "BiomarkerId");

                    b.ToTable("BiomarkerOrders", (string)null);
                });

            modelBuilder.Entity("CE_API_V2.Models.Biomarkers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Aceinhibitor")
                        .HasColumnType("bit");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("AgeUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Alat")
                        .HasColumnType("real");

                    b.Property<string>("AlatUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Albumin")
                        .HasColumnType("real");

                    b.Property<string>("AlbuminUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Alkaline")
                        .HasColumnType("real");

                    b.Property<string>("AlkalineUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Amylasep")
                        .HasColumnType("real");

                    b.Property<string>("AmylasepUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Betablocker")
                        .HasColumnType("bit");

                    b.Property<float>("Bilirubin")
                        .HasColumnType("real");

                    b.Property<string>("BilirubinUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Calciumant")
                        .HasColumnType("bit");

                    b.Property<int>("Chestpain")
                        .HasColumnType("int");

                    b.Property<float>("Cholesterol")
                        .HasColumnType("real");

                    b.Property<string>("CholesterolUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClinicalSetting")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("Diabetes")
                        .HasColumnType("int");

                    b.Property<int>("Diastolicbp")
                        .HasColumnType("int");

                    b.Property<string>("DiastolicbpUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Diuretic")
                        .HasColumnType("bit");

                    b.Property<float>("Glucose")
                        .HasColumnType("real");

                    b.Property<string>("GlucoseUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Hdl")
                        .HasColumnType("real");

                    b.Property<string>("HdlUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("HeightUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Hstroponint")
                        .HasColumnType("real");

                    b.Property<string>("HstroponintUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Ldl")
                        .HasColumnType("real");

                    b.Property<string>("LdlUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Leukocyte")
                        .HasColumnType("real");

                    b.Property<string>("LeukocyteUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Mchc")
                        .HasColumnType("real");

                    b.Property<string>("MchcUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nicotine")
                        .HasColumnType("int");

                    b.Property<bool>("Nitrate")
                        .HasColumnType("bit");

                    b.Property<bool>("PriorCAD")
                        .HasColumnType("bit");

                    b.Property<float>("Protein")
                        .HasColumnType("real");

                    b.Property<string>("ProteinUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Qwave")
                        .HasColumnType("int");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<bool>("Statin")
                        .HasColumnType("bit");

                    b.Property<int>("Systolicbp")
                        .HasColumnType("int");

                    b.Property<string>("SystolicbpUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Tcagginhibitor")
                        .HasColumnType("bit");

                    b.Property<float>("Urea")
                        .HasColumnType("real");

                    b.Property<string>("UreaUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Uricacid")
                        .HasColumnType("real");

                    b.Property<string>("UricacidUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<string>("WeightUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Biomarkers");
                });

            modelBuilder.Entity("CE_API_V2.Models.CountryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CE_API_V2.Models.OrganizationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Userquota")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("CE_API_V2.Models.ScoringRequestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ScoringRequests", (string)null);
                });

            modelBuilder.Entity("CE_API_V2.Models.ScoringResponseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BiomarkersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Recommendation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecommendationLong")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Risk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RiskClass")
                        .HasColumnType("int");

                    b.Property<string>("Score")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("classifier_class")
                        .HasColumnType("int");

                    b.Property<double?>("classifier_score")
                        .HasColumnType("float");

                    b.Property<int?>("classifier_sign")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BiomarkersId")
                        .IsUnique()
                        .HasFilter("[BiomarkersId] IS NOT NULL");

                    b.HasIndex("RequestId");

                    b.ToTable("ScoringResponses", (string)null);
                });

            modelBuilder.Entity("CE_API_V2.Models.UserModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClinicalSetting")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EMailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferredLab")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfessionalSpecialisation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Salutation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitLabValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("CE_API_V2.Models.BiomarkerOrderModel", b =>
                {
                    b.HasOne("CE_API_V2.Models.UserModel", "User")
                        .WithMany("BiomarkerOrders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BiomarkerOrders_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CE_API_V2.Models.Biomarkers", b =>
                {
                    b.HasOne("CE_API_V2.Models.ScoringRequestModel", "Request")
                        .WithMany("Biomarkers")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");
                });

            modelBuilder.Entity("CE_API_V2.Models.ScoringRequestModel", b =>
                {
                    b.HasOne("CE_API_V2.Models.UserModel", "User")
                        .WithMany("ScoringRequestModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CE_API_V2.Models.ScoringResponseModel", b =>
                {
                    b.HasOne("CE_API_V2.Models.Biomarkers", "Biomarkers")
                        .WithOne("Response")
                        .HasForeignKey("CE_API_V2.Models.ScoringResponseModel", "BiomarkersId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("CE_API_V2.Models.ScoringRequestModel", "Request")
                        .WithMany("Responses")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Biomarkers");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("CE_API_V2.Models.Biomarkers", b =>
                {
                    b.Navigation("Response")
                        .IsRequired();
                });

            modelBuilder.Entity("CE_API_V2.Models.ScoringRequestModel", b =>
                {
                    b.Navigation("Biomarkers");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("CE_API_V2.Models.UserModel", b =>
                {
                    b.Navigation("BiomarkerOrders");

                    b.Navigation("ScoringRequestModels");
                });
#pragma warning restore 612, 618
        }
    }
}
