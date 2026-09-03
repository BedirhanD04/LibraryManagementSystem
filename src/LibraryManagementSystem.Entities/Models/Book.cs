namespace LibraryManagementSystem.Entities.Models
{
    // Entity representing book information — will be the "Books" table in the database
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public int AuthorId { get; set; } // Foreign key to the Authors table
        public Author? Author { get; set; } // Navigation property

        public ICollection<Loan> Loans { get; set; } = new List<Loan>(); // Navigation property: a book can have multiple loans (1-N relationship)

    }
}