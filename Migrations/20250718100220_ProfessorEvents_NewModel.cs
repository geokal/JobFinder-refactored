using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorEvents_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorEventProfessorEmail",
                table: "ProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventProfessorImage",
                table: "ProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventProfessorName",
                table: "ProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventProfessorSurName",
                table: "ProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventUniversity",
                table: "ProfessorEvents");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForEventUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorEvents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorEvents_ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents",
                column: "ProfessorEmailUsedToUploadEvent");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorEvents_RNGForEventUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorEvents",
                column: "RNGForEventUploadedAsProfessor_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForEventUploadedAsProfessor_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorEvents_Professors_ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents",
                column: "ProfessorEmailUsedToUploadEvent",
                principalTable: "Professors",
                principalColumn: "ProfEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorEvents_Professors_ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorEvents_ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorEvents_RNGForEventUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailUsedToUploadEvent",
                table: "ProfessorEvents");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForEventUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventProfessorEmail",
                table: "ProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfessorEventProfessorImage",
                table: "ProfessorEvents",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventProfessorName",
                table: "ProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventProfessorSurName",
                table: "ProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventUniversity",
                table: "ProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
