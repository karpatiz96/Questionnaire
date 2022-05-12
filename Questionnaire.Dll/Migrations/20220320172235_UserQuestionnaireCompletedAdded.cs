using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Questionnaire.Dll.Migrations
{
    public partial class UserQuestionnaireCompletedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Finished",
                table: "UserQuestionnaires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "UserQuestionnaires",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RandomQuestionOrder",
                table: "QuestionnaireSheets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "UserQuestionnaires");

            migrationBuilder.DropColumn(
                name: "RandomQuestionOrder",
                table: "QuestionnaireSheets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Finished",
                table: "UserQuestionnaires",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
