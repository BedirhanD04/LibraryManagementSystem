using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Business.DTOs;

// Author information returned by the API
public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
}

// Data from the client for creating a new author — no ID, as the database auto-generates it
public class CreateAuthorDto
{
    [Required(ErrorMessage = "The author name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "The name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }
}
    
       
    
    