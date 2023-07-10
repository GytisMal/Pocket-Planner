using PocketPlanner.Dtos.Categories;
using PocketPlanner.Models;

namespace PocketPlanner.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<ServiceResponse<List<GetCategoryDto>>> GetAllCategories();
        Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<List<GetCategoryDto>>> AddCategory(AddCategoryDto newCategory);
        Task<ServiceResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto updatedCategory);
        Task<ServiceResponse<List<GetCategoryDto>>> DeleteCategory(int id);
    }
}
