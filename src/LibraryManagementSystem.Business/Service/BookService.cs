using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Business.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Author> _authorRepository;

    public BookService(IRepository<Book> bookRepository, IRepository<Author> authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<List<BookDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        var result = new List<BookDto>();
        foreach (var book in books)
        {
            var author = await _authorRepository.GetByIdAsync(book.AuthorId);
            result.Add(MapToDto(book, author?.Name ?? "Unknown Author"));
        }
        return result;
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return null;

        var author = await _authorRepository.GetByIdAsync(book.AuthorId);
        return MapToDto(book, author?.Name ?? "Unknown Author");
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        // Business rule: verify first that the specified AuthorId actually exists
        var author = await _authorRepository.GetByIdAsync(dto.AuthorId);
        if (author == null)
        {
            throw new InvalidOperationException($"Author with ID {dto.AuthorId} does not exist.");
        }

        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies, // All copies of a newly added book are initially available
            AuthorId = dto.AuthorId
        };

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync(); // This is where data is actually written to the

        return MapToDto(book, author.Name);
    }

    public async Task<BookDto?> UpdateAsync(int id, CreateBookDto dto)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return null;

        // Business rule: verify that the specified AuthorId exists
        var author = await _authorRepository.GetByIdAsync(dto.AuthorId);
        if (author == null)
            throw new InvalidOperationException($"Author with ID {dto.AuthorId} does not exist.");
        

        book.Title = dto.Title;
        book.ISBN = dto.ISBN;
        book.TotalCopies = dto.TotalCopies;
        book.AuthorId = dto.AuthorId;

         _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();

        return MapToDto(book, author.Name);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return false;

        _bookRepository.Remove(book);
        await _bookRepository.SaveChangesAsync();
        return true;
    }

    private static BookDto MapToDto(Book book, string authorName) => new()
    {
        Id = book.Id,
        Title = book.Title,
        ISBN = book.ISBN,
        TotalCopies = book.TotalCopies,
        AvailableCopies = book.AvailableCopies,
        AuthorId = book.AuthorId,
        AuthorName = authorName
    };
}
