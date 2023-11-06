namespace FinanceApp.Data.Models;
public class Transaction : BudgetBase
{
    public long TransactionId { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Amount { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }
}
