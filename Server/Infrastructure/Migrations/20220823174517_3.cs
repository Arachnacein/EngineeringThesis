using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_WorkSector_WorkSectorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_WorkSectorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkSectorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "rank",
                table: "Rank",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserWorkSector",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "int", nullable: false),
                    WorkSectorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkSector", x => new { x.UsersId, x.WorkSectorsId });
                    table.ForeignKey(
                        name: "FK_UserWorkSector_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkSector_WorkSector_WorkSectorsId",
                        column: x => x.WorkSectorsId,
                        principalTable: "WorkSector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkSector_WorkSectorsId",
                table: "UserWorkSector",
                column: "WorkSectorsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkSector");

            migrationBuilder.DropColumn(
                name: "rank",
                table: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "WorkSectorId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rank",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_WorkSectorId",
                table: "User",
                column: "WorkSectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_WorkSector_WorkSectorId",
                table: "User",
                column: "WorkSectorId",
                principalTable: "WorkSector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
