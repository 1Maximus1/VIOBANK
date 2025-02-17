using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIOBANK.PostgresPersistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggerFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCard",
                table: "Users",
                column: "IdCard",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TaxNumber",
                table: "Users",
                column: "TaxNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactCard",
                table: "Contacts",
                column: "ContactCard",
                unique: true);

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_account_balance()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Обновляем баланс аккаунта на сумму всех его карт
                    UPDATE ""Accounts""
                    SET ""Balance"" = (
                        SELECT COALESCE(SUM(""Balance""), 0) 
                        FROM ""Cards""
                        WHERE ""AccountId"" = NEW.""AccountId""
                    )
                    WHERE ""AccountId"" = NEW.""AccountId"";

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trigger_update_account_balance
                AFTER INSERT OR UPDATE ON ""Cards""
                FOR EACH ROW
                EXECUTE FUNCTION update_account_balance();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IdCard",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TaxNumber",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ContactCard",
                table: "Contacts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trigger_update_account_balance ON ""Cards"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS update_account_balance;");
        }
    }
}
