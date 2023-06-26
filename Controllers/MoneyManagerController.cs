using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using PocketPlanner.DTO;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace PocketPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PocketPlannerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MoneyManager moneyManager;

        public PocketPlannerController(IConfiguration configuration, MoneyManager moneyManager)
        {
            _configuration = configuration;
            this.moneyManager = moneyManager;
        }

        [HttpGet("transactions")]
        public async Task<List<Transaction>> GetTransactions()
        {
            await moneyManager.ProcessTransactions();

            List<Category> categories = moneyManager.LoadCategories();
            moneyManager.CategorizedTransactions(categories);

            List<Transaction> transactions = moneyManager.GetTransactions();
            return transactions;
        }

        [HttpGet("budget")]
        public List<Budget> GetBudgets()
        {
            List<Budget> budgets = moneyManager.LoadBudgets();
            return budgets;
        }

        [HttpPost("budget")]
        public IActionResult AddBudget([FromBody] Budget budget)
        {
            moneyManager.AddBudget(budget);
            List<Budget> budgets = moneyManager.LoadBudgets();
            return Ok(budgets);
        }

        [HttpPut("budget/{id}")]
        public IActionResult UpdateBudget(string Id, [FromBody] Budget updatedBudget)
        {
            moneyManager.UpdateBudget(Id, updatedBudget);
            List<Budget> budgets = moneyManager.LoadBudgets();
            return Ok(budgets);
        }

        [HttpDelete("budget/{id}")] 
        public IActionResult DeleteBudget(string Id)
        {
            moneyManager.DeleteBudget(Id);
            List<Budget> budgets = moneyManager.LoadBudgets();
            return Ok(budgets);
        }

        [HttpGet("categories")]
        public List<Category> GetCategories()
        {
            List<Category> categories = moneyManager.LoadCategories();
            return categories;
        }

        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            moneyManager.AddCategory(category);
            List<Category> categories = moneyManager.LoadCategories();
            return Ok(categories);
        }

        [HttpPut("categories/{id}")]
        public IActionResult UpdateCategory(string Id, [FromBody] Category updatedCategory)
        {
            moneyManager.UpdateCategory(Id, updatedCategory);
            List<Category> categories = moneyManager.LoadCategories();
            return Ok(categories);
        }

        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(string id)
        {
            moneyManager.DeleteCategory(id);
            List<Category> categories = moneyManager.LoadCategories();
            return Ok(categories);
        }
    }
}

