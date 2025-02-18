using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIOBANK.PostgresPersistence.Migrations
{
    /// <inheritdoc />
    public partial class NewConceptOfBlackList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Token);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_ExpiryDate",
                table: "BlacklistedTokens",
                column: "ExpiryDate");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION cleanup_blacklisted_tokens()
                RETURNS TRIGGER AS $$
                BEGIN
                    DELETE FROM ""BlacklistedTokens""
                    WHERE ""ExpiryDate"" <= NOW();
                    RETURN NULL;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER trigger_cleanup_blacklisted_tokens
                AFTER INSERT OR UPDATE ON ""BlacklistedTokens""
                FOR EACH ROW EXECUTE FUNCTION cleanup_blacklisted_tokens();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedTokens");

            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS trigger_cleanup_blacklisted_tokens ON ""BlacklistedTokens"";
                DROP FUNCTION IF EXISTS cleanup_blacklisted_tokens();
            ");
        }
    }
}
