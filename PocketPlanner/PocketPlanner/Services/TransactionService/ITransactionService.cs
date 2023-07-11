using PocketPlanner.Models;
using System.Text;
using PocketPlanner.Dtos.Transactions;

namespace PocketPlanner.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<ServiceResponse<List<Transaction>>> ProcessTransactions(TransactionDataDto transactionDataDto);
    }
}
