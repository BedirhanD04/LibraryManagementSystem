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
    [Required(ErrorMessage = "Yazar adı zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "İsim 2-200 karakter arasında olmalı.")]
    public string Name { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }
}
    
       
    
    