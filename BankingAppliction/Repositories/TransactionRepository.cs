using BankingAppliction.Data;
using BankingAppliction.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAppliction.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTransactionAsync(Transaction transaction)

        {
            Console.WriteLine(TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time"));
            transaction.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time")); 
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _dbContext.Transactions.FindAsync(transactionId);
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            _dbContext.Transactions.Update(transaction);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // מחיקת פעולה
        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                return false;

            _dbContext.Transactions.Remove(transaction);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(string userId)
        {
            return await _dbContext.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
