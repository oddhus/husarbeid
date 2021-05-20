using Microsoft.EntityFrameworkCore.Migrations;

namespace husarbeid.Data.Migrations
{
    public partial class taskcreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "FamilyTasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyTasks_CreatedById",
                table: "FamilyTasks",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyTasks_Users_CreatedById",
                table: "FamilyTasks",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyTasks_Users_CreatedById",
                table: "FamilyTasks");

            migrationBuilder.DropIndex(
                name: "IX_FamilyTasks_CreatedById",
                table: "FamilyTasks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "FamilyTasks");
        }
    }
}
