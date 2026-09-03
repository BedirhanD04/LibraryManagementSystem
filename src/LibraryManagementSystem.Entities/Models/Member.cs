namespace LibraryManagementSystem.Entities.Models
{
    // Entity representing member of Library — will be the "Members" table in the database
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; } = DateTime.Now; // Default value is the current date and time

        public ICollection<Loan> Loans { get; set; } = new List<Loan>(); // A member can have multiple borrowing records (1-N relationship)
    }
}