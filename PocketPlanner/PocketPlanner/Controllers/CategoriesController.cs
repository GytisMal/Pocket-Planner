using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PocketPlanner.Data;
using PocketPlanner.Services.CategoriesService;
using PocketPlanner.Models;
using PocketPlanner.Services;
using PocketPlanner.Services.TransactionService;

namespace PocketPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(DataContext context, ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryDto>>>> GetAllCategories()
        {
            return Ok(await _categoriesService.GetAllCategories());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> GetCategoryById(int id)
        {
            return Ok(await _categoriesService.GetCategoryById(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryDto>>>> UpdateCategory(UpdateCategoryDto updatedCategory)
        {
            var response = await _categoriesService.UpdateCategory(updatedCategory);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryDto>>>> AddCategory(AddCategoryDto newCategory)
        {
            return Ok(await _categoriesService.AddCategory(newCategory));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> DeleteCategory(int id)
        {
            var response = await _categoriesService.DeleteCategory(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
