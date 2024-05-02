using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TaskTrackers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTrackers_EmployeeId",
                table: "TaskTrackers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTrackers_Employees_EmployeeId",
                table: "TaskTrackers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
