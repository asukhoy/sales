using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "transactions");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "transactions",
                newName: "region");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "transactions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "transactions",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "transactions",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "transactions",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProdId",
                table: "transactions",
                newName: "prod_id");

            migrationBuilder.RenameColumn(
                name: "PricePerUnit",
                table: "transactions",
                newName: "price_per_unit");

            migrationBuilder.RenameColumn(
                name: "PriceInCurrency",
                table: "transactions",
                newName: "price_in_currency");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "region",
                table: "Transactions",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Transactions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Transactions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "Transactions",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "Transactions",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "prod_id",
                table: "Transactions",
                newName: "ProdId");

            migrationBuilder.RenameColumn(
                name: "price_per_unit",
                table: "Transactions",
                newName: "PricePerUnit");

            migrationBuilder.RenameColumn(
                name: "price_in_currency",
                table: "Transactions",
                newName: "PriceInCurrency");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");
        }
    }
}
