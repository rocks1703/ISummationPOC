using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISummationPOC.Migrations
{
    /// <inheritdoc />
    public partial class fixess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usertypeid",
                table: "mst_user",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usertypeid",
                table: "mst_user");
        }
    }
}
