using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Business.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? MembershipDate { get; set; }
}

public class CreateMemberDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}