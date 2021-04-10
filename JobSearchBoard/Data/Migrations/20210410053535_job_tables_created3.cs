using Microsoft.EntityFrameworkCore.Migrations;

namespace JobSearchBoard.Data.Migrations
{
    public partial class job_tables_created3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobPostings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_UserId",
                table: "JobPostings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_AspNetUsers_UserId",
                table: "JobPostings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_AspNetUsers_UserId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_UserId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobPostings");
        }
    }
}
