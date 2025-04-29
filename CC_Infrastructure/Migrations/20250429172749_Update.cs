using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RetrievedAt",
                table: "FinancialData",
                newName: "RaportDate");

            migrationBuilder.AddColumn<decimal>(
                name: "Revenue",
                table: "FinancialData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "FinancialData");

            migrationBuilder.RenameColumn(
                name: "RaportDate",
                table: "FinancialData",
                newName: "RetrievedAt");
        }
    }
}
