using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorInternshipApplied_NewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheProfessorByProfessor",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheProfessorByStudent",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternsnipTitleAppliedAtTheProfessor",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailAppliedForInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorInternshipPositionTypeApplied",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorNameAppliedForInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnameAppliedForInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentCVAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentImageAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentNameAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentRegNumberAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentSurnameAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RNGForProfessorInternshipApplied_HashedAsUniqueID",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheProfessorSide",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheStudentSide",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailWhereStudentAppliedForInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniqueIDWhereStudentAppliedForInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProfessorInternshipsApplied_ProfessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfessorUniqueIDWhereStudentAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorEmailWhereStudentAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorInternshipsApplied_ProfessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorInternshipsApplied_ProfessorDetails_ProfessorInternshipsApplied_Id",
                        column: x => x.Id,
                        principalTable: "ProfessorInternshipsApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorInternshipsApplied_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentAppliedForProfessorInternship = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForProfessorInternshipAppliedAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorInternshipsApplied_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorInternshipsApplied_StudentDetails_ProfessorInternshipsApplied_Id",
                        column: x => x.Id,
                        principalTable: "ProfessorInternshipsApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorInternshipsApplied_StudentEmailAppliedForProfessorInternship_RNGForProfessorInternshipApplied",
                table: "ProfessorInternshipsApplied",
                columns: new[] { "StudentEmailAppliedForProfessorInternship", "RNGForProfessorInternshipApplied" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorInternshipsApplied_ProfessorDetails_ProfessorEmailWhereStudentAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied_ProfessorDetails",
                column: "ProfessorEmailWhereStudentAppliedForProfessorInternship");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorInternshipsApplied_StudentDetails_StudentEmailAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied_StudentDetails",
                column: "StudentEmailAppliedForProfessorInternship");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessorInternshipsApplied_ProfessorDetails");

            migrationBuilder.DropTable(
                name: "ProfessorInternshipsApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorInternshipsApplied_StudentEmailAppliedForProfessorInternship_RNGForProfessorInternshipApplied",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheProfessorSide",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheStudentSide",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailWhereStudentAppliedForInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorUniqueIDWhereStudentAppliedForInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForProfessorInternshipApplied_HashedAsUniqueID",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheProfessorByProfessor",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheProfessorByStudent",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternsnipTitleAppliedAtTheProfessor",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailAppliedForInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorInternshipPositionTypeApplied",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameAppliedForInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnameAppliedForInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentCVAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentRegNumberAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneAppliedForProfessorInternship",
                table: "ProfessorInternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
