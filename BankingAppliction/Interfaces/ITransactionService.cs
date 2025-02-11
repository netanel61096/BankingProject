using BankingAppliction.Models;
using System.Threading.Tasks;

namespace BankingAppliction.Services
{
    public interface ITransactionService
    {
        Task<TransactionResult> ExecuteTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(string userId);
        Task<TransactionResult> UpdateTransactionAsync(int transactionId, decimal newAmount, string newBankAccount);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}

