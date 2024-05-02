using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChnagedAssigneetoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTrackers_Employees_Asignee",
                table: "TaskTrackers");

            migrationBuilder.DropIndex(
                name: "IX_TaskTrackers_Asignee",
                table: "TaskTrackers");

            migrationBuilder.DropColumn(
                name: "Asignee",
                table: "TaskTrackers");

            migrationBuilder.AlterColumn<string>(
                name: "Assignee",
                table: "TaskTrackers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Assignee",
                table: "TaskTrackers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Asignee",
                table: "TaskTrackers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_Asignee",
                table: "TaskTrackers",
                column: "Asignee");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Employees_Asignee",
                table: "TaskTrackers",
                column: "Asignee",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
