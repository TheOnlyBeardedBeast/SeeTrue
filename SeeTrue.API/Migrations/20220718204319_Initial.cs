using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeTrue.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Payload = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    Template = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Audience = table.Column<string>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstanceID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Aud = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    EncryptedPassword = table.Column<string>(type: "TEXT", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InvitedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConfirmationToken = table.Column<string>(type: "TEXT", nullable: true),
                    ConfirmationSentAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RecoveryToken = table.Column<string>(type: "TEXT", nullable: true),
                    RecoverySentAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailChangeToken = table.Column<string>(type: "TEXT", nullable: true),
                    EmailChange = table.Column<string>(type: "TEXT", nullable: true),
                    EmailChangeSentAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSignInAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AppMetaData = table.Column<string>(type: "TEXT", nullable: true),
                    UserMetaData = table.Column<string>(type: "TEXT", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstanceID = table.Column<Guid>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Revoked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Logins_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Logins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_InstanceId_Language_Type_Audience",
                table: "Mails",
                columns: new[] { "InstanceId", "Language", "Type", "Audience" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_LoginId",
                table: "RefreshTokens",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_InstanceID_Aud",
                table: "Users",
                columns: new[] { "Email", "InstanceID", "Aud" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogEntries");

            migrationBuilder.DropTable(
                name: "Mails");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
