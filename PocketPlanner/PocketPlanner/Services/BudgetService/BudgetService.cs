global using AutoMapper;
using PocketPlanner.Dtos.Budget;
using PocketPlanner.Models;

namespace PocketPlanner.Services.BudgetService
{
    public class BudgetService : IBudgetService
    {
        private static List<Budget> budgets = new List<Budget>
        {
            new Budget(),
            new Budget { Id = 1, Type = "Food", Amount = 400, Date = DateTime.Parse("2023-05-01") },
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ITransactionService _transactionService;

        public BudgetService(IMapper mapper, DataContext context, ITransactionService transactionService)
        {
            _mapper = mapper;
            _context = context;
            _transactionService = transactionService;
        }

        public async Task<ServiceResponse<List<GetBudgetDto>>> AddBudget(AddBudgetDto newBudget)
        {
            var serviceResponse = new ServiceResponse<List<GetBudgetDto>>();
            var budget = _mapper.Map<Budget>(newBudget);

            var dbBudgets = await _context.Budget.ToListAsync();

            _context.Budget.Add(budget);
            await _context.SaveChangesAsync();

            var dbBudget = await _context.Budget.ToListAsync();
            serviceResponse.Data = dbBudget.Select(budget => _mapper.Map<GetBudgetDto>(budget)).ToList();

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

                var remainingBudget = await _context.Budget.ToListAsync();

                if (remainingBudget.Count == 0)
                {
                    // Reset the identity seed to 1
                    await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Budget', RESEED, 0)");
                }
                else if (remainingBudget.Count != 0)
                {
                    // Get the maximum existing ID
                    var maxId = remainingBudget.Max(b => b.Id);

                    // Reset the identity seed to the maximum ID
                    await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('Budget', RESEED, {maxId})");
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
    }
}
