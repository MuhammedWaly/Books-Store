using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginAndRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class initAddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FristName], [LastName], [ProfilePic]) VALUES (N'6b7f1382-c22b-4a22-8bee-e3faa52d55be', N'Admin', N'ADMIN', N'Admin@Admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAED+IuIKyUTYoeEwtguYEKh0nY7PE/GU++BfkCNYeWbAs/RJFFX+h5MZeJ2UlYfSEWA==', N'SA7RSBX7HGVB224QWRYLBLJWRY5EVRJH', N'9c6cc308-75b2-4c62-860c-33e0012377ba', NULL, 0, 0, NULL, 1, 0, N'Muhammed', N'Waly', Null)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '6b7f1382-c22b-4a22-8bee-e3faa52d55be'");
        }
    }
}
