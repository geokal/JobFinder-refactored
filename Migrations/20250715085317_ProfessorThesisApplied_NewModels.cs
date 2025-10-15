using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorThesisApplied_NewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorAppliedForThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailAppliedForThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorThesisStatusApplied",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorThesisStatusAppliedAtTheProfessor",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorThesisTypeApplied",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentCVAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentImageAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentNameAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentRegNumberAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentSurnameAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ThesisInProfessorApplied",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ThesisTitleAppliedAtTheProfessor",
                table: "ProfessorThesesApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RNGForProfessorThesisApplied_HashedAsUniqueID",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailWhereStudentAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorThesisStatusAppliedAtProfessorSide",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorThesisStatusAppliedAtStudentSide",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniqueIDWhereStudentAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProfessorThesisApplied_ProfessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfessorUniqueIDWhereStudentAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorEmailWhereStudentAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorThesisApplied_ProfessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorThesisApplied_ProfessorDetails_ProfessorThesesApplied_Id",
                        column: x => x.Id,
                        principalTable: "ProfessorThesesApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorThesisApplied_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentAppliedForProfessorThesis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForProfessorThesisApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorThesisApplied_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorThesisApplied_StudentDetails_ProfessorThesesApplied_Id",
                        column: x => x.Id,
                        principalTable: "ProfessorThesesApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorThesesApplied_StudentEmailAppliedForProfessorThesis_RNGForProfessorThesisApplied",
                table: "ProfessorThesesApplied",
                columns: new[] { "StudentEmailAppliedForProfessorThesis", "RNGForProfessorThesisApplied" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorThesisApplied_ProfessorDetails_ProfessorEmailWhereStudentAppliedForProfessorThesis",
                table: "ProfessorThesisApplied_ProfessorDetails",
                column: "ProfessorEmailWhereStudentAppliedForProfessorThesis");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorThesisApplied_StudentDetails_StudentEmailAppliedForProfessorThesis",
                table: "ProfessorThesisApplied_StudentDetails",
                column: "StudentEmailAppliedForProfessorThesis");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessorThesisApplied_ProfessorDetails");

            migrationBuilder.DropTable(
                name: "ProfessorThesisApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorThesesApplied_StudentEmailAppliedForProfessorThesis_RNGForProfessorThesisApplied",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailWhereStudentAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorThesisStatusAppliedAtProfessorSide",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorThesisStatusAppliedAtStudentSide",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "ProfessorUniqueIDWhereStudentAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDAppliedForProfessorThesis",
                table: "ProfessorThesesApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForProfessorThesisApplied_HashedAsUniqueID",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorAppliedForThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailAppliedForThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorThesisStatusApplied",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorThesisStatusAppliedAtTheProfessor",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorThesisTypeApplied",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentCVAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentRegNumberAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneAppliedForProfessorThesis",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThesisInProfessorApplied",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThesisTitleAppliedAtTheProfessor",
                table: "ProfessorThesesApplied",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
