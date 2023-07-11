using PocketPlanner.Models;

namespace PocketPlanner
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, GetCategoryDto>();
            CreateMap<GetCategoryDto, Category>();
            CreateMap<AddCategoryDto, Category>();

            CreateMap<ServiceResponse<List<GetCategoryDto>>, ServiceResponse<List<Category>>>();

            CreateMap<Budget, GetBudgetDto>();
            CreateMap<AddBudgetDto, Budget>();

            CreateMap<Transaction, GetTransactionDto>();
            CreateMap<GetTransactionDto, Transaction>();
        }
    }
}
