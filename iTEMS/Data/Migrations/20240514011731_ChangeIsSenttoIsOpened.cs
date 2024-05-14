using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsSenttoIsOpened : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpened",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpened",
                table: "Notifications");
        }
    }
}
