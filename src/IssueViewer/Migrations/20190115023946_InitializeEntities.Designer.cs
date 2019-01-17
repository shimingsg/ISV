﻿// <auto-generated />
using System;
using IssueViewer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IssueViewer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190115023946_InitializeEntities")]
    partial class InitializeEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("IssueViewer.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("IssueViewer.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<DateTime?>("ClosedDateTime");

                    b.Property<DateTime?>("CreatedDateTime");

                    b.Property<int>("IssueId");

                    b.Property<DateTime?>("LastUpdatedDateTime");

                    b.Property<string>("Link");

                    b.Property<string>("RepoIdentier");

                    b.Property<int?>("State");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("IssueViewer.Models.Category", b =>
                {
                    b.HasOne("IssueViewer.Models.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("IssueViewer.Models.Issue", b =>
                {
                    b.HasOne("IssueViewer.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}