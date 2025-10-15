using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorAnnouncements_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorAnnouncementProfessorName",
                table: "AnnouncementsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorAnnouncementProfessorSurname",
                table: "AnnouncementsAsProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAsProfessor_ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor",
                column: "ProfessorAnnouncementProfessorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAsProfessor_ProfessorAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsProfessor",
                column: "ProfessorAnnouncementRNG_HashedAsUniqueID",
                unique: true,
                filter: "[ProfessorAnnouncementRNG_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementsAsProfessor_Professors_ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor",
                column: "ProfessorAnnouncementProfessorEmail",
                principalTable: "Professors",
                principalColumn: "ProfEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementsAsProfessor_Professors_ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementsAsProfessor_ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementsAsProfessor_ProfessorAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorAnnouncementProfessorEmail",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorAnnouncementProfessorName",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorAnnouncementProfessorSurname",
                table: "AnnouncementsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
