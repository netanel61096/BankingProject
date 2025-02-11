using BankingAppliction.Models;

namespace BankingAppliction.Repositories
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
        Task<bool> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(string userId);
    }
}
