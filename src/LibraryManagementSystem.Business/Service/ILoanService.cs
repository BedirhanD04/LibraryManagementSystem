using LibraryManagementSystem.Business.DTOs;

namespace LibraryManagementSystem.Business.Services;

public interface ILoanService
{
    Task<LoanDto> CreateLoanAsync(int memberId, int bookId);
    Task ReturnBookAsync(int loanId);
    Task<List<LoanDto>> GetOverdueLoansAsync();
}