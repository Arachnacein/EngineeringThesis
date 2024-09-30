using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class MigrationCheckedByAdmin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsChechedByAdmin",
                table: "Swap",
                newName: "IsCheckedByAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCheckedByAdmin",
                table: "Swap",
                newName: "IsChechedByAdmin");
        }
    }
}
