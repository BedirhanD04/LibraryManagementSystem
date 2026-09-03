using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.DataAccess.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<List<Loan>> GetActiveLoansByMemberIdAsync(int memberId);
    Task<List<Loan>> GetOverdueLoansAsync();
    Task<int> CountActiveLoansByMemberAsync(int memberId);
}