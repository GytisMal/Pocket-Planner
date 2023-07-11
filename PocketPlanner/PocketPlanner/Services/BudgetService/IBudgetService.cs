using PocketPlanner.Dtos.Budget;
using PocketPlanner.Models;

namespace PocketPlanner.Services.BudgetService
{
    public interface IBudgetService
    {
        Task<ServiceResponse<List<GetBudgetDto>>> GetAllBudget();
        Task<ServiceResponse<GetBudgetDto>> GetBudgetById(int id);
        Task<ServiceResponse<List<GetBudgetDto>>> AddBudget(AddBudgetDto newBudget);
        Task<ServiceResponse<GetBudgetDto>> UpdateBudget(UpdateBudgetDto updatedBudget);
        Task<ServiceResponse<List<GetBudgetDto>>> DeleteBudget(int id);
    }
}