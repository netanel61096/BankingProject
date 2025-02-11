using BankingAppliction.Models;
using BankingAppliction.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppliction.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteTransaction([FromBody] Transaction transaction)
        {
            try
            {
                var result = await _transactionService.ExecuteTransactionAsync(transaction);

                if (result.Success)
                {
                    return Ok(new { message = "Transaction completed successfully" });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Transaction failed",
                        error = result.ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה כללית: {ex.Message}");
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }


        [HttpPut("update/{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionRequest request)
        {
            try
            {
                var result = await _transactionService.UpdateTransactionAsync(transactionId, request.NewAmount, request.NewBankAccount);
                if (!result.Success)
                    return BadRequest(new { message = result.ErrorMessage }); 

                return Ok(new { message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" שגיאה כללית בעדכון פעולה {transactionId}: {ex.Message}");
                return StatusCode(500, new { message = " שגיאה בעת עדכון הפעולה", error = ex.Message });
            }
        }
        [HttpDelete("delete/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            try
            {
                var success = await _transactionService.DeleteTransactionAsync(transactionId);
                if (!success)
                    return NotFound(new { message = " הפעולה לא נמצאה" });

                return Ok(new { message = "הפעולה נמחקה בהצלחה" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה במחיקת פעולה {transactionId}: {ex.Message}");
                return StatusCode(500, new { message = "❌ שגיאה בעת מחיקת הפעולה", error = ex.Message });
            }
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetTransactionHistory(string userId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionHistoryAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" שגיאה בקבלת היסטוריית פעולות: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving transaction history." });
            }
        }
    }
}
