using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LessonsAPI.Migrations
{
    /// <inheritdoc />
    public partial class db9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Lesson_LessonId1",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_LessonId1",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "LessonId1",
                table: "Task");

            migrationBuilder.AlterColumn<long>(
                name: "LessonId",
                table: "Task",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_LessonId",
                table: "Task",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Lesson_LessonId",
                table: "Task",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Lesson_LessonId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_LessonId",
                table: "Task");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Task",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LessonId1",
                table: "Task",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_LessonId1",
                table: "Task",
                column: "LessonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Lesson_LessonId1",
                table: "Task",
                column: "LessonId1",
                principalTable: "Lesson",
                principalColumn: "Id");
        }
    }
}
