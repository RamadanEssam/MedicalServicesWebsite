using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //to add admin when project start up
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfileImg], [SSN]) VALUES (N'8987b900-bc3e-4221-9d52-2e5e3f9c536e', N'mostafa.fathy85.mf', N'MOSTAFA.FATHY85.MF', N'mostafa.fathy85.mf@gmail.com', N'MOSTAFA.FATHY85.MF@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAECATCYZpzDXTwrxrplE5XbuPwApulWz0CmDJNbiaAYogsh8/oU67luG1b1E4UQZ8SQ==', N'BGV5OXDRUB3QBNRECPEU5TSWSNIO7BFF', N'b3e5c59d-a7d7-48bc-ae07-74f0cdc9643c', NULL, 0, 0, NULL, 1, 0, N'mostafa', N'fathy', NULL, 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '8987b900-bc3e-4221-9d52-2e5e3f9c536e'");
        }
    }
}
