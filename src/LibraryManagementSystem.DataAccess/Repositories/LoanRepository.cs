using LibraryManagementSystem.DataAccess.Data;
using LibraryManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DataAccess.Repositories;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Loan>> GetActiveLoansByMemberIdAsync(int memberId) =>
        await _context.Loans
            .Where(l => l.MemberId == memberId && l.ReturnDate == null)
            .ToListAsync();

    public async Task<List<Loan>> GetOverdueLoansAsync() =>
        await _context.Loans
            .Where(l => l.ReturnDate == null && l.DueDate < DateTime.UtcNow)
            .ToListAsync();

    public async Task<int> CountActiveLoansByMemberAsync(int memberId) =>
        await _context.Loans.CountAsync(l => l.MemberId == memberId && l.ReturnDate == null);        
}