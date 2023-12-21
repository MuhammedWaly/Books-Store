using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginAndRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class initAddAdmin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT into [security].[UserRoles] (UserId,RoleId) SELECT '6b7f1382-c22b-4a22-8bee-e3faa52d55be', Id FROM [security].Roles ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE from [security].[UserRoles] WHERE UserId='6b7f1382-c22b-4a22-8bee-e3faa52d55be'");
        }
    }
}
