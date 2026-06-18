using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class EnrollProjectsUsersupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف العلاقات القديمة (Cascade)
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_EnrollProjects_porjectID",
                table: "EnrollProjectsUsers");

            // ✅ إضافة Rating فقط
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "EnrollProjectsUsers",
                type: "int",
                nullable: true);

            // ✅ إعادة العلاقات بدون Cascade (Restrict)
            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_EnrollProjects_porjectID",
                table: "EnrollProjectsUsers",
                column: "porjectID",
                principalTable: "EnrollProjects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // حذف العلاقات
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollProjectsUsers_EnrollProjects_porjectID",
                table: "EnrollProjectsUsers");

            // حذف العمود
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "EnrollProjectsUsers");

            // رجوع العلاقات القديمة (Cascade)
            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_AspNetUsers_userID",
                table: "EnrollProjectsUsers",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollProjectsUsers_EnrollProjects_porjectID",
                table: "EnrollProjectsUsers",
                column: "porjectID",
                principalTable: "EnrollProjects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}