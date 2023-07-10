namespace PocketPlanner.Dtos.Budget
{
    public class AddBudgetDto
    {
        public string? Type { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
