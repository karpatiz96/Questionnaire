using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Questionnaire.Dll.Migrations
{
    public partial class UserQuestionnaireDateTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Finished",
                table: "UserQuestionnaires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Started",
                table: "UserQuestionnaires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "AnswerEvaluated",
                table: "UserQuestionnaireAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Completed",
                table: "UserQuestionnaireAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "UserQuestionnaires");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "UserQuestionnaires");

            migrationBuilder.DropColumn(
                name: "AnswerEvaluated",
                table: "UserQuestionnaireAnswers");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "UserQuestionnaireAnswers");
        }
    }
}
