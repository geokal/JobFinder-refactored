using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyEvents_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyEventCompanyEmail",
                table: "CompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventCompanyLogo",
                table: "CompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventCompanyName",
                table: "CompanyEvents");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForEventUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyEvents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEvents_CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents",
                column: "CompanyEmailUsedToUploadEvent");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEvents_RNGForEventUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyEvents",
                column: "RNGForEventUploadedAsCompany_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForEventUploadedAsCompany_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyEvents_Companies_CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents",
                column: "CompanyEmailUsedToUploadEvent",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyEvents_Companies_CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents");

            migrationBuilder.DropIndex(
                name: "IX_CompanyEvents_CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents");

            migrationBuilder.DropIndex(
                name: "IX_CompanyEvents_RNGForEventUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEmailUsedToUploadEvent",
                table: "CompanyEvents");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForEventUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventCompanyEmail",
                table: "CompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CompanyEventCompanyLogo",
                table: "CompanyEvents",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventCompanyName",
                table: "CompanyEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
