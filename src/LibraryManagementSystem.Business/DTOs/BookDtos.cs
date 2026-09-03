using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Business.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty; // Only the author's name, not the entire Author object

}

public class CreateBookDto
{
    [Required, StringLength(300, MinimumLength = 1, ErrorMessage = "Title cannot be empty.")]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 5, ErrorMessage = "Please enter a valid ISBN.")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, 10000, ErrorMessage = "Number of copies must be between 1 and 10000.")]
    public int TotalCopies { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid author.")]
    public int AuthorId { get; set; }
}