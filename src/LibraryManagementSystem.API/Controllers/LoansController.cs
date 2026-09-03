using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> Create([FromQuery] int memberId, [FromQuery] int bookId)
    {
        try
        {
            var loan = await _loanService.CreateLoanAsync(memberId, bookId);
            return Ok(loan);        
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/return")]
    public async Task<ActionResult> ReturnBook(int id)
    {
        try
        {
            await _loanService.ReturnBookAsync(id);
            return NoContent(); // 204 No Content
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<List<LoanDto>>> GetOverdueL()
    {
        var loans = await _loanService.GetOverdueLoansAsync();
        return Ok(loans);
    }

}