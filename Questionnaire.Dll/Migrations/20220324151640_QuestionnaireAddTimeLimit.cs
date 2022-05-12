using Microsoft.EntityFrameworkCore.Migrations;

namespace Questionnaire.Dll.Migrations
{
    public partial class QuestionnaireAddTimeLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LimitedTime",
                table: "QuestionnaireSheets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "QuestionnaireSheets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitedTime",
                table: "QuestionnaireSheets");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "QuestionnaireSheets");
        }
    }
}
