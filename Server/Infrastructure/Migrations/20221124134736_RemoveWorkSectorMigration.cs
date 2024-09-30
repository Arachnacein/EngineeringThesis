using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class RemoveWorkSectorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_WorkSector_WorkSectorsId",
                table: "Schedule");

            migrationBuilder.DropTable(
                name: "UserWorkSector");

            migrationBuilder.DropTable(
                name: "WorkSector");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_WorkSectorsId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Id_WorkSector",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Id_WorkSector1",
                table: "Swap");

            migrationBuilder.DropColumn(
                name: "Id_WorkSector2",
                table: "Swap");

            migrationBuilder.DropColumn(
                name: "Id_WorkSector",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "WorkSectorsId",
                table: "Schedule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_WorkSector",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_WorkSector1",
                table: "Swap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_WorkSector2",
                table: "Swap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_WorkSector",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkSectorsId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkSector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSector", x => x.Id);
                });

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
                name: "IX_Schedule_WorkSectorsId",
                table: "Schedule",
                column: "WorkSectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkSector_WorkSectorsId",
                table: "UserWorkSector",
                column: "WorkSectorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_WorkSector_WorkSectorsId",
                table: "Schedule",
                column: "WorkSectorsId",
                principalTable: "WorkSector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
