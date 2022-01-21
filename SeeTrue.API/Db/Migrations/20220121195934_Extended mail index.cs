using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeTrue.API.Db.Migrations
{
    public partial class Extendedmailindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Mails_Language_Type_Audience",
                table: "Mails");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstanceID",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstanceId",
                table: "Mails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Mails_InstanceId_Language_Type_Audience",
                table: "Mails",
                columns: new[] { "InstanceId", "Language", "Type", "Audience" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Mails_InstanceId_Language_Type_Audience",
                table: "Mails");

            migrationBuilder.DropColumn(
                name: "InstanceId",
                table: "Mails");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstanceID",
                table: "Users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_Language_Type_Audience",
                table: "Mails",
                columns: new[] { "Language", "Type", "Audience" },
                unique: true);
        }
    }
}
