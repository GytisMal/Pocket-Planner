namespace PocketPlanner.Dtos.Budget
{
    public class UpdateBudgetDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
