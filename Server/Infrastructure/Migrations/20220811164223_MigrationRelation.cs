using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class MigrationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "WorkSectorId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DutiesId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkSectorsId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DutiesId",
                table: "PersonalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PersonalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ContractTypeId",
                table: "User",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RankId",
                table: "User",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_User_WorkSectorId",
                table: "User",
                column: "WorkSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_DutiesId",
                table: "Schedule",
                column: "DutiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_UsersId",
                table: "Schedule",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_WorkSectorsId",
                table: "Schedule",
                column: "WorkSectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRequests_DutiesId",
                table: "PersonalRequests",
                column: "DutiesId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRequests_UserId",
                table: "PersonalRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalRequests_Duty_DutiesId",
                table: "PersonalRequests",
                column: "DutiesId",
                principalTable: "Duty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalRequests_User_UserId",
                table: "PersonalRequests",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Duty_DutiesId",
                table: "Schedule",
                column: "DutiesId",
                principalTable: "Duty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_User_UsersId",
                table: "Schedule",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_WorkSector_WorkSectorsId",
                table: "Schedule",
                column: "WorkSectorsId",
                principalTable: "WorkSector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_WorkSector_WorkSectorId",
                table: "User",
                column: "WorkSectorId",
                principalTable: "WorkSector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalRequests_Duty_DutiesId",
                table: "PersonalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalRequests_User_UserId",
                table: "PersonalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Duty_DutiesId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_User_UsersId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_WorkSector_WorkSectorsId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_User_ContractType_ContractTypeId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Rank_RankId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_WorkSector_WorkSectorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ContractTypeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RankId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_WorkSectorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_DutiesId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_UsersId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_WorkSectorsId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_PersonalRequests_DutiesId",
                table: "PersonalRequests");

            migrationBuilder.DropIndex(
                name: "IX_PersonalRequests_UserId",
                table: "PersonalRequests");

            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkSectorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DutiesId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "WorkSectorsId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DutiesId",
                table: "PersonalRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalRequests");
        }
    }
}
