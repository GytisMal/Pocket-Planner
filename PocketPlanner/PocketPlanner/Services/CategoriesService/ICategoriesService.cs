using PocketPlanner.Models;
using PocketPlanner.Dtos.Categories;

namespace PocketPlanner.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<ServiceResponse<List<GetCategoryDto>>> GetAllCategories();
        Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto newCategory);
        Task<ServiceResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto updatedCategory);
        Task<ServiceResponse<List<GetCategoryDto>>> DeleteCategory(int id);
    }
}
