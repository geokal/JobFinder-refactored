using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyJobs_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyNameUploadJob",
                table: "CompanyJobs");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForPositionUploaded_HashedAsUniqueID",
                table: "CompanyJobs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailUsedToUploadJobs",
                table: "CompanyJobs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmail",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Companies_CompanyEmail",
                table: "Companies",
                column: "CompanyEmail");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobs_EmailUsedToUploadJobs",
                table: "CompanyJobs",
                column: "EmailUsedToUploadJobs");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobs_RNGForPositionUploaded_HashedAsUniqueID",
                table: "CompanyJobs",
                column: "RNGForPositionUploaded_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForPositionUploaded_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJobs_Companies_EmailUsedToUploadJobs",
                table: "CompanyJobs",
                column: "EmailUsedToUploadJobs",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobs_Companies_EmailUsedToUploadJobs",
                table: "CompanyJobs");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobs_EmailUsedToUploadJobs",
                table: "CompanyJobs");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobs_RNGForPositionUploaded_HashedAsUniqueID",
                table: "CompanyJobs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Companies_CompanyEmail",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForPositionUploaded_HashedAsUniqueID",
                table: "CompanyJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailUsedToUploadJobs",
                table: "CompanyJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameUploadJob",
                table: "CompanyJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmail",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
