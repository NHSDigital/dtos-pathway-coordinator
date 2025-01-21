﻿// <auto-generated />
using System;
using ContextManager.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContextManager.API.Migrations
{
    [DbContext(typeof(ContextManagerDbContext))]
    [Migration("20250120111933_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContextManager.Old.Models.Participant", b =>
                {
                    b.Property<string>("NhsNumber")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NhsNumber");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("ContextManager.Old.Models.Pathway", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParticipantNhsNumber")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.HasIndex("ParticipantNhsNumber");

                    b.ToTable("Pathways");
                });

            modelBuilder.Entity("PathwayCoordinator.Models.GenericEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NhsNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pathway")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathwayName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PathwayName");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ContextManager.Old.Models.Pathway", b =>
                {
                    b.HasOne("ContextManager.Old.Models.Participant", null)
                        .WithMany("Pathways")
                        .HasForeignKey("ParticipantNhsNumber");
                });

            modelBuilder.Entity("PathwayCoordinator.Models.GenericEvent", b =>
                {
                    b.HasOne("ContextManager.Old.Models.Pathway", null)
                        .WithMany("Events")
                        .HasForeignKey("PathwayName");
                });

            modelBuilder.Entity("ContextManager.Old.Models.Participant", b =>
                {
                    b.Navigation("Pathways");
                });

            modelBuilder.Entity("ContextManager.Old.Models.Pathway", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
