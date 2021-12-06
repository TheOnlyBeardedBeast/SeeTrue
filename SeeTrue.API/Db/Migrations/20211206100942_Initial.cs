using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeTrue.API.Db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Payload = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstanceID = table.Column<Guid>(type: "uuid", nullable: true),
                    Aud = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EncryptedPassword = table.Column<string>(type: "text", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvitedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConfirmationToken = table.Column<string>(type: "text", nullable: true),
                    ConfirmationSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecoveryToken = table.Column<string>(type: "text", nullable: true),
                    RecoverySentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmailChangeToken = table.Column<string>(type: "text", nullable: true),
                    EmailChange = table.Column<string>(type: "text", nullable: true),
                    EmailChangeSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSignInAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AppMetaDataJson = table.Column<string>(type: "jsonb", nullable: true),
                    UserMetaDataJson = table.Column<string>(type: "jsonb", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogEntries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
