using LibraryManagementSystem.Business.DTOs;

namespace LibraryManagementSystem.Business.Services;

public interface IBookService
{
    Task<List<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(CreateBookDto dto);
}