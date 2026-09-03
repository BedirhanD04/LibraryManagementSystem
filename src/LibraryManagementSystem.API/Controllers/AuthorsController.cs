using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController] // Provides conveniences like automatic model validation and automatic [FromBody] binding
[Route("api/[controller]")] // "[controller]" -> strips "Controller" from the class name -> "api/Authors

public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet] // GET /api/authors
    public async Task<ActionResult<List<AuthorDto>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors); // 200
    }

    [HttpGet("{id}")] // GET /api/authors/5
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
             return NotFound();
        
        return Ok(author);
    }

    [HttpPost] // POST /api/authors
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto dto)
    {
        var created = await _authorService.CreateAsync(dto);
        // 201 Created + "Location" header pointing to GetById (REST standard)
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}