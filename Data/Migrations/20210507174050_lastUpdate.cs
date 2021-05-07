using Microsoft.EntityFrameworkCore.Migrations;

namespace husarbeid.Data.Migrations
{
    public partial class lastUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "FamilyTasks",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "FamilyTasks");
        }
    }
}
