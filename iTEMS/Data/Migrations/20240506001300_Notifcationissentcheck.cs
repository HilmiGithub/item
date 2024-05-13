using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTEMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Notifcationissentcheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Notifications");
        }
    }
}
