using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssigneetoEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedTo",
                table: "TaskTrackers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_AssignedTo",
                table: "TaskTrackers",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Employees_AssignedTo",
                table: "TaskTrackers",
                column: "AssignedTo",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTrackers_Employees_AssignedTo",
                table: "TaskTrackers");

            migrationBuilder.DropIndex(
                name: "IX_TaskTrackers_AssignedTo",
                table: "TaskTrackers");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "TaskTrackers");
        }
    }
}
