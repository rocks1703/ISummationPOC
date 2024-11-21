using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class addedrecstatuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "recstatus",
                table: "mst_user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recstatus",
                table: "mst_user");
        }
    }
}
