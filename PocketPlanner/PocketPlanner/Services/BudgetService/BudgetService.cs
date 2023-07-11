global using AutoMapper;
using PocketPlanner.Models;
using PocketPlanner.Dtos.Budget;
using PocketPlanner.Models;

namespace PocketPlanner.Services.BudgetService
{
    public class BudgetService : IBudgetService
    {
        private static List<Budget> budgets = new List<Budget>();
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public BudgetService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetBudgetDto>> AddBudget(AddBudgetDto newBudget)
        {
            var serviceResponse = new ServiceResponse<GetBudgetDto>();
            var budget = _mapper.Map<Budget>(newBudget);

            _context.Budget.Add(budget);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetBudgetDto>(budget);

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetBudgetDto>>> GetAllBudget()
        {
            var serviceResponse = new ServiceResponse<List<GetBudgetDto>>();
            var dbBudget = await _context.Budget.ToListAsync();
            serviceResponse.Data = dbBudget.Select(budget => _mapper.Map<GetBudgetDto>(budget)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBudgetDto>> GetBudgetById(int id)
        {
            var serviceResponse = new ServiceResponse<GetBudgetDto>();
            var dbBudget = await _context.Budget.FirstOrDefaultAsync(budget => budget.Id == id);
            serviceResponse.Data = _mapper.Map<GetBudgetDto>(dbBudget);
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetBudgetDto>> UpdateBudget(UpdateBudgetDto updatedBudget)
        {
            var serviceResponse = new ServiceResponse<GetBudgetDto>();
            try
            {
                var budget = await _context.Budget.FindAsync(updatedBudget.Id);
                if (budget is null)
                    throw new Exception($"Budget with Id '{updatedBudget.Id}' not found.");

                budget.Type = updatedBudget.Type;
                budget.Amount = updatedBudget.Amount;
                budget.Date = updatedBudget.Date;
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetBudgetDto>(budget);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBudgetDto>>> DeleteBudget(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetBudgetDto>>();
            try
            {
                var budget = await _context.Budget.FirstOrDefaultAsync(budget => budget.Id == id);
                if (budget is null)
                    throw new Exception($"Budget with Id '{id}' not found.");

                _context.Budget.Remove(budget);
                await _context.SaveChangesAsync();

                var remainingBudgets = await _context.Budget.ToListAsync();

                if (remainingBudgets.Count == 0)
                {
                    // Reset the identity seed to 1
                    await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Budgets', RESEED, 0)");
                }
                else if (remainingBudgets.Count != 0)
                {
                    // Get the maximum existing ID
                    var maxId = remainingBudgets.Max(b => b.Id);

                    // Reset the identity seed to the maximum ID
                    await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('Budgets', RESEED, {maxId})");
                }

                var dbBudget = await _context.Budget.ToListAsync();
                serviceResponse.Data = dbBudget.Select(budget => _mapper.Map<GetBudgetDto>(budget)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<Dictionary<string, double>> GetBudgetTotalsByCategory()
        {
            var totalsByCategory = await _context.Transactions
                .Where(transaction => !string.IsNullOrEmpty(transaction.Category))
                .GroupBy(transaction => transaction.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    Amount = Math.Round(group.Sum(transaction => transaction.Amount), 2)
                })
                .ToDictionaryAsync(transaction => transaction.Category, transaction => transaction.Amount);

            return totalsByCategory;
        }

        public async Task<Dictionary<string, double>> GetBudgetBalance()
        {
            var budgetTotalsByCategory = await GetBudgetTotalsByCategory();
            var budgetBalance = new Dictionary<string, double>();

            var budgets = await _context.Budget.ToListAsync();

            foreach (Budget budget in budgets)
            {
                var category = budget.Type;
                var budgetAmount = budget.Amount;
                var spentAmount = budgetTotalsByCategory.ContainsKey(category) ? budgetTotalsByCategory[category] : 0;
                var balance = budgetAmount - spentAmount;

                budgetBalance.Add(category, balance);
            }

            foreach (var roundingNumbers in budgetBalance)
            {
                budgetBalance[roundingNumbers.Key] = Math.Round(roundingNumbers.Value, 2);
            }

            return budgetBalance;
        }
    }
}