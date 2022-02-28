using Microsoft.EntityFrameworkCore.Migrations;

namespace Questionnaire.Dll.Migrations
{
    public partial class QuestionCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QuestionCompleted",
                table: "UserQuestionnaireAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionCompleted",
                table: "UserQuestionnaireAnswers");
        }
    }
}
