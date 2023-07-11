namespace PocketPlanner.Dtos.Budget
{
    public class GetBudgetDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public double Amount { get; set; }
        public string? Date { get; set; }
    }
}
