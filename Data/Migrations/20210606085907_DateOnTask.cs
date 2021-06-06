using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace husarbeid.Data.Migrations
{
    public partial class DateOnTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdOn",
                table: "FamilyTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedOn",
                table: "FamilyTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdOn",
                table: "FamilyTasks");

            migrationBuilder.DropColumn(
                name: "updatedOn",
                table: "FamilyTasks");
        }
    }
}
