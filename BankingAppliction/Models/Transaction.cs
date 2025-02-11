using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAppliction.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string FullNameHebrew { get; set; }  
        public string FullNameEnglish { get; set; }
        public DateTime DateOfBirth { get; set; }   
        public string UserId { get; set; }
        public string ActionType { get; set; }
        public decimal Amount { get; set; }
        public string BankAccount { get; set; }
        public string Status { get; set; } = "Pending";


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time"));
    }
}
