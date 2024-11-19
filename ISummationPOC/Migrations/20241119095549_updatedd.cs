using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class updatedd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usertypeid",
                table: "mst_user");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "mst_user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "mst_user");

            migrationBuilder.AddColumn<int>(
                name: "usertypeid",
                table: "mst_user",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
