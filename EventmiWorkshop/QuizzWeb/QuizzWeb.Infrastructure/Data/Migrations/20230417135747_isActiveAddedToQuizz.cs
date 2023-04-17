using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizzWeb.Data.Migrations
{
    public partial class isActiveAddedToQuizz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Маркиран ли е като изтрит");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Quizzes");
        }
    }
}
