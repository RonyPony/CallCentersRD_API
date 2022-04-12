using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCentersRD_API.Migrations
{
    public partial class editResponsesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "responserName",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "responserName",
                table: "Responses");
        }
    }
}
