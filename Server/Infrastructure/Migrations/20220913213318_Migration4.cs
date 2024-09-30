using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ContractType_ContractTypeId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Rank_RankId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ContractType");

            migrationBuilder.DropTable(
                name: "Rank");

            migrationBuilder.DropIndex(
                name: "IX_User_ContractTypeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RankId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "WorkTime",
                table: "Duty",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkTime",
                table: "Duty");

            migrationBuilder.AddColumn<int>(
                name: "ContractTypeId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ContractTypeId",
                table: "User",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RankId",
                table: "User",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ContractType_ContractTypeId",
                table: "User",
                column: "ContractTypeId",
                principalTable: "ContractType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Rank_RankId",
                table: "User",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
