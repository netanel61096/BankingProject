using Microsoft.AspNetCore.Mvc;
using ExternalBankingAPI.Models;
using System;

[ApiController]
[Route("api/externalBanking")]
public class ExternalBankingController : ControllerBase
{

    [HttpPost("createtoken")]
    public IActionResult CreateToken([FromBody] TokenRequest request)
    {
        try
        {
            if (request.UserId.Length == 9)
            {
                return Ok(new { code = "SUCCESS", data = Guid.NewGuid().ToString() });
            }
            return BadRequest(new { code = "ERROR", message = "❌ Invalid UserId. UserId must be 9 digits." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in CreateToken: {ex.Message}");
            return StatusCode(500, new { code = "ERROR", message = "❌ Internal Server Error", error = ex.Message });
        }
    }


    [HttpPost("createdeposit")]
    public IActionResult CreateDeposit([FromBody] TransactionRequest request)
    {
        try
        {
            if (request.Amount <= 0)
                return BadRequest(new { code = "ERROR", message = "❌ Amount must be greater than zero." });

            if (request.BankAccount.Length > 10)
                return BadRequest(new { code = "ERROR", message = "❌ BankAccount must be up to 10 digits." });

            return Ok(new { code = "SUCCESS", data = "✅ Deposit Approved" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in CreateDeposit: {ex.Message}");
            return StatusCode(500, new { code = "ERROR", message = "❌ Internal Server Error", error = ex.Message });
        }
    }


    [HttpPost("createWithdrawal")]
    public IActionResult CreateWithdrawal([FromBody] TransactionRequest request)
    {
        try
        {
            if (request.Amount <= 0)
                return BadRequest(new { code = "ERROR", message = "❌ Amount must be greater than zero." });

            if (request.BankAccount.Length > 10)
                return BadRequest(new { code = "ERROR", message = "❌ BankAccount must be up to 10 digits." });

            return Ok(new { code = "SUCCESS", data = "✅ Withdrawal Approved" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in CreateWithdrawal: {ex.Message}");
            return StatusCode(500, new { code = "ERROR", message = "❌ Internal Server Error", error = ex.Message });
        }
    }
}
