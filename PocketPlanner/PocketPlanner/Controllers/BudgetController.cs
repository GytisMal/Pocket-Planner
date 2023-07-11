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
using PocketPlanner;
using PocketPlanner.Data;
using PocketPlanner.Services.BudgetService;
using PocketPlanner.Models;
using PocketPlanner.Services;
using PocketPlanner.Dtos.Budget;

namespace PocketPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetBudgetDto>>>> GetAllBudget()
        {
            return Ok(await _budgetService.GetAllBudget());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBudgetDto>>>> GetBudgetById(int id)
        {
            return Ok(await _budgetService.GetBudgetById(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBudgetDto>>>> UpdateBudget(UpdateBudgetDto updatedBudget)
        {
            var response = await _budgetService.UpdateBudget(updatedBudget);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetBudgetDto>>>> AddBudget(AddBudgetDto newBudget)
        {
            return Ok(await _budgetService.AddBudget(newBudget));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetBudgetDto>>> DeleteBudget(int id)
        {
            var response = await _budgetService.DeleteBudget(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("spent")]
        public async Task<ActionResult<Dictionary<string, double>>> GetBudgetTotalsByCategory()
        {
            var response = await _budgetService.GetBudgetTotalsByCategory();
            if (response is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("balance")]
        public async Task<ActionResult<Dictionary<string, double>>> GetBudgetBalance()
        {
            var response = await _budgetService.GetBudgetBalance();
            if (response is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
