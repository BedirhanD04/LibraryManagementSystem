using LibraryManagementSystem.Business.DTOs;

namespace LibraryManagementSystem.Business.Services;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(CreateAuthorDto dto);
}