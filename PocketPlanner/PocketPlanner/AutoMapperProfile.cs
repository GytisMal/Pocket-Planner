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

            CreateMap<Budget, GetBudgetDto>()
                .ForMember(change => change.Date,
                    changeTo => changeTo.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
            CreateMap<AddBudgetDto, Budget>();

            CreateMap<Transaction, GetTransactionDto>();
            CreateMap<GetTransactionDto, Transaction>();
        }
    }
}
