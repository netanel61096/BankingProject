namespace BankingAppliction.Models
{
    public class TransactionResult
    {
        public bool Success { get; }
        public string ErrorMessage { get; }

        public TransactionResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}

