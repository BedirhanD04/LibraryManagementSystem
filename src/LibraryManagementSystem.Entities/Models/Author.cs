namespace LibraryManagementSystem.Entities.Models
{
    // Entity representing author information — will be the "Authors" table in the database
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; } // ? = Not Necessary

        // Navigation property: an author can have multiple books (1-N relationship)
        // When EF Core detects this, it automatically adds an AuthorId foreign key to the Books table
        public ICollection<Book> Books { get; set; } = new List<Book>(); // Initialize the collection to avoid null reference issues
    }
}