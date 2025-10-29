using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MvdBackend.Migrations
{
    /// <inheritdoc />
    public partial class SplitAiAnalysisToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiAnalyzedAt",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "AiCategory",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "AiPriority",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "AiSentiment",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "AiSuggestedAction",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "AiSummary",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "FinalCategory",
                table: "CitizenRequests");

            migrationBuilder.DropColumn(
                name: "IsAiCorrected",
                table: "CitizenRequests");

            migrationBuilder.CreateTable(
                name: "CitizenRequestAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CitizenRequestId = table.Column<int>(type: "integer", nullable: false),
                    AiCategory = table.Column<string>(type: "text", nullable: true),
                    AiPriority = table.Column<string>(type: "text", nullable: true),
                    AiSummary = table.Column<string>(type: "text", nullable: true),
                    AiSuggestedAction = table.Column<string>(type: "text", nullable: true),
                    AiSentiment = table.Column<string>(type: "text", nullable: true),
                    AiAnalyzedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAiCorrected = table.Column<bool>(type: "boolean", nullable: false),
                    FinalCategory = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizenRequestAnalyses_CitizenRequests_CitizenRequestId",
                        column: x => x.CitizenRequestId,
                        principalTable: "CitizenRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestAnalyses_CitizenRequestId",
                table: "CitizenRequestAnalyses",
                column: "CitizenRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CitizenRequestAnalyses");

            migrationBuilder.AddColumn<DateTime>(
                name: "AiAnalyzedAt",
                table: "CitizenRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiCategory",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiPriority",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiSentiment",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiSuggestedAction",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiSummary",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalCategory",
                table: "CitizenRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAiCorrected",
                table: "CitizenRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
