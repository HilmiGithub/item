using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFkEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTrackers_Employees_EmployeeId",
                table: "TaskTrackers");

            migrationBuilder.DropIndex(
                name: "IX_TaskTrackers_EmployeeId",
                table: "TaskTrackers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TaskTrackers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TaskTrackers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_EmployeeId",
                table: "TaskTrackers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Employees_EmployeeId",
                table: "TaskTrackers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
