using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_EnrollProjects_ProjectId",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_EnrollProjects_ProjectId",
                table: "Notification",
                column: "ProjectId",
                principalTable: "EnrollProjects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_EnrollProjects_ProjectId",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_EnrollProjects_ProjectId",
                table: "Notification",
                column: "ProjectId",
                principalTable: "EnrollProjects",
                principalColumn: "ProjectID");
        }
    }
}
