using Microsoft.AspNetCore.Mvc;
using PocketPlanner.Dtos.Transactions;
using PocketPlanner.Services.TransactionService;
using PocketPlanner.Dtos.Transactions;
using PocketPlanner.Models;
using PocketPlanner.Services.TransactionService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/Transactions
        //[HttpGet]
        //public async Task<ActionResult<List<GetTransactionDto>>> Get()
        //{
        //    var serviceResponse = await _transactionService.GetAllTransactions();
        //    if (serviceResponse.Success)
        //    {
        //        return Ok(serviceResponse.Data);
        //    }
        //    else
        //    {
        //        return BadRequest(serviceResponse.Message);
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Transaction>>>> Post([FromBody] TransactionDataDto transactionDataDto)
        {
            var serviceResponse = await _transactionService.ProcessTransactions(transactionDataDto);
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse.Data);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }
    }
}