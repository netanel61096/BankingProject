using Microsoft.AspNetCore.Mvc;
using ExternalBankingAPI.Models;


[ApiController]
[Route("api/externalBanking")]
public class ExternalBankingController : ControllerBase
{
    // קריאה לקבלת טוקן
    [HttpPost("createtoken")]
    public IActionResult CreateToken([FromBody] TokenRequest request)
    {
        if (request.UserId.Length == 9)
        {
            return Ok(new { code = "SUCCESS", data = Guid.NewGuid().ToString() });
        }
        return BadRequest(new { code = "ERROR", message = "Invalid UserId" });
    }

    // קריאה לביצוע הפקדה
    [HttpPost("createdeposit")]
    public IActionResult CreateDeposit([FromBody] TransactionRequest request)
    {
        if (request.Amount > 0 && request.BankAccount.Length <= 10)
        {
            return Ok(new { code = "SUCCESS", data = "Deposit Approved" });
        }
        return BadRequest(new { code = "ERROR", message = "Invalid deposit details" });
    }

    // קריאה לביצוע משיכה
    [HttpPost("createWithdrawal")]
    public IActionResult CreateWithdrawal([FromBody] TransactionRequest request)
    {
        if (request.Amount > 0 && request.BankAccount.Length <= 10)
        {
            return Ok(new { code = "SUCCESS", data = "Withdrawal Approved" });
        }
        return BadRequest(new { code = "ERROR", message = "Invalid withdrawal details" });
    }
}

