﻿// <auto-generated />
using System;
using MapsVisualisation.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MapsVisualisation.Database.Migrations
{
    [DbContext(typeof(MapsVisualisationContext))]
    [Migration("20221119134339_OtherSourcesAdded2")]
    partial class OtherSourcesAdded2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.Map", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("CollectionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dpi")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PublishYear")
                        .HasColumnType("int");

                    b.Property<Guid?>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.OtherSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("OtherSources");
                });

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<double>("NELat")
                        .HasColumnType("float");

                    b.Property<double>("NELong")
                        .HasColumnType("float");

                    b.Property<double>("NWLat")
                        .HasColumnType("float");

                    b.Property<double>("NWLong")
                        .HasColumnType("float");

                    b.Property<string>("RegionIdentity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionName1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionName2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionName3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SELat")
                        .HasColumnType("float");

                    b.Property<double>("SELong")
                        .HasColumnType("float");

                    b.Property<double>("SWLat")
                        .HasColumnType("float");

                    b.Property<double>("SWLong")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.Map", b =>
                {
                    b.HasOne("MapsVisualisation.Domain.Entities.Region", "Region")
                        .WithMany("Maps")
                        .HasForeignKey("RegionId");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.OtherSource", b =>
                {
                    b.HasOne("MapsVisualisation.Domain.Entities.Region", "Region")
                        .WithMany("OtherSources")
                        .HasForeignKey("RegionId");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("MapsVisualisation.Domain.Entities.Region", b =>
                {
                    b.Navigation("Maps");

                    b.Navigation("OtherSources");
                });
#pragma warning restore 612, 618
        }
    }
}
