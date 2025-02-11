namespace BankingAppliction.Models
{
    public class UpdateTransactionRequest
    {
        public decimal NewAmount { get; set; }
        public string NewBankAccount { get; set; }
    }
}
