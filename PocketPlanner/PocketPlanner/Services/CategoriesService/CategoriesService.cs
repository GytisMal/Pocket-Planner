global using AutoMapper;
using PocketPlanner.Models;

namespace PocketPlanner.Services.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        private static List<Category> categories = new List<Category>();
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CategoriesService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetCategoryDto>> AddCategory(AddCategoryDto newCategory)
        {
            var serviceResponse = new ServiceResponse<GetCategoryDto>();
            var category = _mapper.Map<Category>(newCategory);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCategoryDto>(category);

            return serviceResponse;
        }


        public async Task<ServiceResponse<List<GetCategoryDto>>> GetAllCategories()
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryDto>>();
            var dbCategories = await _context.Categories.ToListAsync();
            serviceResponse.Data = dbCategories.Select(category => _mapper.Map<GetCategoryDto>(category)).ToList();
            return serviceResponse;
        }


        public async Task<ServiceResponse<GetCategoryDto>> GetCategoryById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCategoryDto>();
            var dbCategory = await _context.Categories.FirstOrDefaultAsync(category => category.Id == id);
            serviceResponse.Data = _mapper.Map<GetCategoryDto>(dbCategory);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto updatedCategory)
        {
            var serviceResponse = new ServiceResponse<GetCategoryDto>();
            try
            {
                var category = await _context.Categories.FindAsync(updatedCategory.Id);
                if (category is null)
                    throw new Exception($"Category with Id '{updatedCategory.Id}' not found.");

                category.Name = updatedCategory.Name;
                category.Pattern = updatedCategory.Pattern;
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCategoryDto>(category);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCategoryDto>>> DeleteCategory(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryDto>>();
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(category => category.Id == id);
                if (category is null)
                    throw new Exception($"Category with Id '{id}' not found.");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                var remainingCategories = await _context.Categories.ToListAsync();

                if (remainingCategories.Count == 0)
                {
                    await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Categories', RESEED, 0)");
                }
                else if (remainingCategories.Count != 0)
                {
                    var maxId = remainingCategories.Max(category => category.Id);
                    await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('Categories', RESEED, {maxId})");
                }

                var dbCategories = await _context.Categories.ToListAsync();
                serviceResponse.Data = dbCategories.Select(category => _mapper.Map<GetCategoryDto>(category)).ToList();
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