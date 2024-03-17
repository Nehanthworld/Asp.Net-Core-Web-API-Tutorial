using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class CreatingFKRolePrilegeRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_RoleId",
                table: "RolePrivileges",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePrivileges_Roles",
                table: "RolePrivileges",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePrivileges_Roles",
                table: "RolePrivileges");

            migrationBuilder.DropIndex(
                name: "IX_RolePrivileges_RoleId",
                table: "RolePrivileges");
        }
    }
}
