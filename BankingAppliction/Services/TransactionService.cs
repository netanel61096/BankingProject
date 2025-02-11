using BankingAppliction.Models;
using BankingAppliction.Repositories;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace BankingAppliction.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly HttpClient _httpClient;

        public TransactionService(ITransactionRepository transactionRepository, HttpClient httpClient)
        {
            _transactionRepository = transactionRepository;
            _httpClient = httpClient;
        }

        public async Task<TransactionResult> ExecuteTransactionAsync(Transaction transaction)
        {
            Console.WriteLine($"🔹 התחלת פעולה עבור משתמש: {transaction.UserId}");

            var requestBody = new { UserId = transaction.UserId };

            if (transaction.ActionType != "deposit" && transaction.ActionType != "withdrawal")
            {
                return new TransactionResult(false, "❌ סוג הפעולה אינו תקין. יש לבחור בין 'deposit' או 'withdrawal'.");
            }

            if (!Regex.IsMatch(transaction.UserId, @"^\d{9}$")) // בדיוק 9 ספרות
            {
                return new TransactionResult(false, "❌ מספר תעודת זהות חייב להכיל בדיוק 9 ספרות וללא תווים מיוחדים.");
            }

            if (  transaction.Amount <= 0 || !Regex.IsMatch(transaction.Amount.ToString(), @"^\d{1,10}$")) // בדיקה שסכום תקין
            {
                return new TransactionResult(false, "❌  סכום חייב להיות מספר חוקי וחיובי בלבד ועד 10 ספרות.");
            }

            if (!Regex.IsMatch(transaction.BankAccount, @"^\d{1,10}$")) 
            {
                return new TransactionResult(false, "❌ מספר חשבון בנק חייב להכיל עד 10 ספרות וללא תווים מיוחדים.");
            }
            if (!Regex.IsMatch(transaction.FullNameHebrew, @"^[א-ת\s'\-]{1,20}$")) 
            {
                return new TransactionResult(false, "❌ השם בעברית חייב להכיל רק אותיות בעברית, עד 20 תווים ומותר להשתמש בגרש (-) ורווח.");
            }

            if (!Regex.IsMatch(transaction.FullNameEnglish, @"^[A-Za-z\s'\-]{1,20}$")) 
            {
                return new TransactionResult(false, "❌ השם באנגלית חייב להכיל רק אותיות באנגלית, עד 20 תווים ומותר להשתמש בגרש (-) ורווח.");
            }

            if (!DateTime.TryParseExact(transaction.DateOfBirth.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return new TransactionResult(false, "❌ תאריך הלידה חייב להיות בפורמט YYYY-MM-DD.");
            }
            try
            {
                var tokenResponse = await _httpClient.PostAsJsonAsync("https://localhost:44393/api/externalBanking/createtoken", requestBody);

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return new TransactionResult(false, "❌ שגיאה בקבלת טוקן מהספק החיצוני.");
                }

                var tokenResult = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>();

                if (tokenResult == null || tokenResult.Code != "SUCCESS")
                {
                    return new TransactionResult(false, "❌ הטוקן שהתקבל אינו תקף.");
                }

                Console.WriteLine($"✅ קיבלנו טוקן: {tokenResult.Data}");

                // בחירת URL לפי סוג הפעולה
                string actionUrl = transaction.ActionType == "deposit"
                    ? "https://localhost:44393/api/externalBanking/createdeposit"
                    : "https://localhost:44393/api/externalBanking/createWithdrawal";

                var response = await _httpClient.PostAsJsonAsync(actionUrl, transaction);

                if (!response.IsSuccessStatusCode)
                {
                    transaction.Status = "Failed";
                    await _transactionRepository.AddTransactionAsync(transaction);
                    return new TransactionResult(false, "❌ הפעולה נכשלה - קיבלנו תגובת שגיאה מהספק החיצוני.");
                }

                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                transaction.Status = result?.Code == "SUCCESS" ? "Success" : "Failed";

                await _transactionRepository.AddTransactionAsync(transaction);
                return transaction.Status == "Success"
                    ? new TransactionResult(true, null)
                    : new TransactionResult(false, "❌ הפעולה נכשלה אצל הספק החיצוני.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה כללית בביצוע הפעולה: {ex.Message}");
                transaction.Status = "Failed";
                await _transactionRepository.AddTransactionAsync(transaction);
                return new TransactionResult(false, $"❌ שגיאה כללית: {ex.Message}");
            }
        }

        public async Task<TransactionResult> UpdateTransactionAsync(int transactionId, decimal newAmount, string newBankAccount)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
                if (transaction == null)
                    return new TransactionResult(false, "❌ הפעולה לא נמצאה.");

                // ולידציה לסכום
                if (!Regex.IsMatch(newAmount.ToString(), @"^\d{1,10}$") || newAmount <= 0)
                    return new TransactionResult(false, "❌ סכום חייב להיות מספר חוקי, עד 10 ספרות וללא תווים מיוחדים.");

                // ולידציה לחשבון בנק
                if (!Regex.IsMatch(newBankAccount, @"^\d{1,10}$"))
                    return new TransactionResult(false, "❌ מספר חשבון בנק חייב להכיל עד 10 ספרות וללא תווים מיוחדים.");

                // בדיקה אם הנתונים החדשים זהים לנתונים הישנים
                if (transaction.Amount == newAmount && transaction.BankAccount == newBankAccount)
                    return new TransactionResult(false, "❌ לא התבצע שינוי. יש להזין ערכים חדשים השונים מהקיימים.");

                // עדכון הנתונים
                transaction.Amount = newAmount;
                transaction.BankAccount = newBankAccount;

                var updateSuccess = await _transactionRepository.UpdateTransactionAsync(transaction);
                if (!updateSuccess)
                    return new TransactionResult(false, "❌ שגיאה בעת שמירת הנתונים במסד הנתונים.");

                return new TransactionResult(true, "✅ הפעולה עודכנה בהצלחה.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה כללית בעדכון פעולה {transactionId}: {ex.Message}");
                return new TransactionResult(false, $"❌ שגיאה בלתי צפויה: {ex.Message}");
            }
        }


        // מחיקת פעולה
        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            return await _transactionRepository.DeleteTransactionAsync(transactionId);
        }


        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(string userId)
        {
            return await _transactionRepository.GetTransactionHistoryAsync(userId);
        }
    }
}
