using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollegeName",
                table: "ProfileInfo");

            migrationBuilder.AddColumn<int>(
                name: "CollegeId",
                table: "ProfileInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "College",
                columns: table => new
                {
                    CollegeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_College", x => x.CollegeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfo_CollegeId",
                table: "ProfileInfo",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInfo_College_CollegeId",
                table: "ProfileInfo",
                column: "CollegeId",
                principalTable: "College",
                principalColumn: "CollegeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInfo_College_CollegeId",
                table: "ProfileInfo");

            migrationBuilder.DropTable(
                name: "College");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInfo_CollegeId",
                table: "ProfileInfo");

            migrationBuilder.DropColumn(
                name: "CollegeId",
                table: "ProfileInfo");

            migrationBuilder.AddColumn<string>(
                name: "CollegeName",
                table: "ProfileInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
