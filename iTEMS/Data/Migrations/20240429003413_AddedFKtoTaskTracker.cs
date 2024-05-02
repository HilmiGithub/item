using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKtoTaskTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
