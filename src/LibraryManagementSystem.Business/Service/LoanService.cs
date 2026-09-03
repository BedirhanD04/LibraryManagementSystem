using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Business.Services;

public class LoanService : ILoanService
{
    private const int maxActiveLoansPerMember = 3; // Maximum number of active loans allowed per member
    private const int loanDurationDays = 14; // Loan duration in days

    private readonly ILoanRepository _loanRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Member> _memberRepository;

    public LoanService(ILoanRepository loanRepository, IRepository<Book> bookRepository, IRepository<Member> memberRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public async Task<LoanDto> CreateLoanAsync(int memberId, int bookId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            throw new InvalidOperationException("Member not found.");

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        // Rule 1: Stock check
        if(book.AvailableCopies <= 0)
            throw new InvalidOperationException("No available copies of the book.");

        // Rule 2: Member limit check
        var activeLoans = await _loanRepository.CountActiveLoansByMemberAsync(memberId);
        if (activeLoans >= maxActiveLoansPerMember)
            throw new InvalidOperationException("Member has reached the maximum number of active loans.");
    

        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(loanDurationDays),
        };

        book.AvailableCopies -= 1; // Decrease available copies
        _bookRepository.Update(book); // Update the book's available copies

        await _loanRepository.AddAsync(loan);
        await _loanRepository.SaveChangesAsync(); // Both the Loan addition and Book update are saved at the same time

        return new LoanDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = book.Title,
            MemberId = member.Id,
            MemberName = member.Name,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate
        };
    }

    public async Task ReturnBookAsync(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null)
            throw new InvalidOperationException("Loan not found.");

        if (loan.IsReturned)
            throw new InvalidOperationException("Book has already been returned.");

        loan.ReturnDate = DateTime.UtcNow;

        var book = await _bookRepository.GetByIdAsync(loan.BookId);
        if (book != null)
        {
            book.AvailableCopies++;  // Increase available copies
            _bookRepository.Update(book); // Update the book's available copies
        }

        _loanRepository.Update(loan);
        await _loanRepository.SaveChangesAsync(); // Both the Loan update and Book update are saved at the same time
    }

    public async Task<List<LoanDto>> GetOverdueLoansAsync()
    {
        var overdueLoans = await _loanRepository.GetOverdueLoansAsync();
        var result = new List<LoanDto>();

        foreach (var loan in overdueLoans)
        {
            var book = await _bookRepository.GetByIdAsync(loan.BookId);
            var member = await _memberRepository.GetByIdAsync(loan.MemberId);

            result.Add(new LoanDto
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = book?.Title ?? "Unknown",
                MemberId = loan.MemberId,
                MemberName = member?.Name ?? "Unknown",
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate
            });
        }

        return result;
    }

}