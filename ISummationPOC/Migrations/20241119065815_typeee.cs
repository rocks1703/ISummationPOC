using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class typeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usertype_id",
                table: "mst_user");

            migrationBuilder.AddColumn<string>(
                name: "usertype",
                table: "mst_user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usertype",
                table: "mst_user");

            migrationBuilder.AddColumn<int>(
                name: "usertype_id",
                table: "mst_user",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
