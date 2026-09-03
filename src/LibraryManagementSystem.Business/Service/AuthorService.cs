using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Business.Services;

public class AuthorService : IAuthorService
{
    private readonly IRepository<Author> _authorRepository;

    public AuthorService(IRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<List<AuthorDto>> GetAllAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(MapToDto).ToList();
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        return author == null ? null : MapToDto(author);
    }

    public async Task<AuthorDto> CreateAsync(CreateAuthorDto createAuthorDto)
    {
        var author = new Author
        {
            Name = createAuthorDto.Name,
            DateOfBirth = createAuthorDto.DateOfBirth
        };

        await _authorRepository.AddAsync(author);
        await _authorRepository.SaveChangesAsync(); // This is where data is actually written to the database

        return MapToDto(author);

    }

    // Centralizing Entity -> DTO mapping in one place to prevent code duplication
    private static AuthorDto MapToDto(Author author) => new()
    {
        Id = author.Id,
        Name = author.Name,
        DateOfBirth = author.DateOfBirth
    };
    
       
}    