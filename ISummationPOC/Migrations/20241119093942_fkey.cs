using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class fkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "mst_user",
                newName: "usertype");

            migrationBuilder.AlterColumn<string>(
                name: "usertype",
                table: "mst_user",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "usertype",
                table: "mst_user",
                newName: "UserType");

            migrationBuilder.AlterColumn<int>(
                name: "UserType",
                table: "mst_user",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
