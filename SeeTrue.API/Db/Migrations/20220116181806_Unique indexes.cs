using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeTrue.API.Db.Migrations
{
    public partial class Uniqueindexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_InstanceID_Aud",
                table: "Users",
                columns: new[] { "Email", "InstanceID", "Aud" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mails_Language_Type_Audience",
                table: "Mails",
                columns: new[] { "Language", "Type", "Audience" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email_InstanceID_Aud",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Mails_Language_Type_Audience",
                table: "Mails");
        }
    }
}
