using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ResearchGroup_Patents1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchGroup_Patents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_UniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_Patent_PatentURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_Patent_PatentTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_Patent_PatentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_Patent_PatentDOI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_Patent_PatentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_Patents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchGroup_SpinOffCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_UniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_SpinOff_CompanyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_SpinOff_CompanyAFM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_SpinOff_CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_SpinOffCompany", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchGroup_Patents");

            migrationBuilder.DropTable(
                name: "ResearchGroup_SpinOffCompany");
        }
    }
}
