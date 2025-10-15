using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyAnnouncements_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyAnnouncementCompanyName",
                table: "AnnouncementsAsCompany");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsCompany",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAsCompany_CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany",
                column: "CompanyAnnouncementCompanyEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAsCompany_CompanyAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsCompany",
                column: "CompanyAnnouncementRNG_HashedAsUniqueID",
                unique: true,
                filter: "[CompanyAnnouncementRNG_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementsAsCompany_Companies_CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany",
                column: "CompanyAnnouncementCompanyEmail",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementsAsCompany_Companies_CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementsAsCompany_CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementsAsCompany_CompanyAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsCompany");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyAnnouncementRNG_HashedAsUniqueID",
                table: "AnnouncementsAsCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyAnnouncementCompanyEmail",
                table: "AnnouncementsAsCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAnnouncementCompanyName",
                table: "AnnouncementsAsCompany",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
