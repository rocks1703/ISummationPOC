﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class fixesss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usertype",
                table: "mst_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usertype",
                table: "mst_user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
