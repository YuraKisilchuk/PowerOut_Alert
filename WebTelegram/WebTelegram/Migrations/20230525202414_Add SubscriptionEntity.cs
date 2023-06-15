using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebTelegram.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExclusionEntityId",
                table: "tblExclusions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExclusionId = table.Column<int>(type: "integer", nullable: false),
                    TelegramChatId = table.Column<int>(type: "integer", nullable: false),
                    SendOffInvite = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SendOnInvite = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSubscriptions_tblExclusions_ExclusionId",
                        column: x => x.ExclusionId,
                        principalTable: "tblExclusions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblSubscriptions_tblTelegramChates_TelegramChatId",
                        column: x => x.TelegramChatId,
                        principalTable: "tblTelegramChates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblExclusions_ExclusionEntityId",
                table: "tblExclusions",
                column: "ExclusionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSubscriptions_ExclusionId",
                table: "tblSubscriptions",
                column: "ExclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSubscriptions_TelegramChatId",
                table: "tblSubscriptions",
                column: "TelegramChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblExclusions_tblExclusions_ExclusionEntityId",
                table: "tblExclusions",
                column: "ExclusionEntityId",
                principalTable: "tblExclusions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblExclusions_tblExclusions_ExclusionEntityId",
                table: "tblExclusions");

            migrationBuilder.DropTable(
                name: "tblSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_tblExclusions_ExclusionEntityId",
                table: "tblExclusions");

            migrationBuilder.DropColumn(
                name: "ExclusionEntityId",
                table: "tblExclusions");
        }
    }
}
