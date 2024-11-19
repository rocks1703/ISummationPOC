﻿// <auto-generated />
using ISummationPOC.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ISummationPOC.Migrations
{
    [DbContext(typeof(ISummationDbContext))]
    [Migration("20241119101615_fixess")]
    partial class fixess
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ISummationPOC.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("lastname");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("mobile");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("profileimage");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usertype");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int")
                        .HasColumnName("usertypeid");

                    b.HasKey("Id");

                    b.ToTable("mst_user");
                });

            modelBuilder.Entity("ISummationPOC.Entity.UserTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usertype");

                    b.HasKey("Id");

                    b.ToTable("mst_usertypes");
                });
#pragma warning restore 612, 618
        }
    }
}
