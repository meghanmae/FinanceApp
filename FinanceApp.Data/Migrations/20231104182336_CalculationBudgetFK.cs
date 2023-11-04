using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalculationBudgetFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "CustomCalculations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomCalculations_BudgetId",
                table: "CustomCalculations",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomCalculations_Budgets_BudgetId",
                table: "CustomCalculations",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomCalculations_Budgets_BudgetId",
                table: "CustomCalculations");

            migrationBuilder.DropIndex(
                name: "IX_CustomCalculations_BudgetId",
                table: "CustomCalculations");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "CustomCalculations");
        }
    }
}
