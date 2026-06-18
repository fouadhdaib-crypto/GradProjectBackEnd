using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prize",
                table: "EnrollProjects");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EnrollProjects");

            migrationBuilder.AddColumn<string>(
                name: "ProjectLocation",
                table: "EnrollProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamType",
                table: "EnrollProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numberOFAvailableSeats",
                table: "EnrollProjects",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectLocation",
                table: "EnrollProjects");

            migrationBuilder.DropColumn(
                name: "TeamType",
                table: "EnrollProjects");

            migrationBuilder.DropColumn(
                name: "numberOFAvailableSeats",
                table: "EnrollProjects");

            migrationBuilder.AddColumn<string>(
                name: "Prize",
                table: "EnrollProjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EnrollProjects",
                type: "datetime2",
                nullable: true);
        }
    }
}
