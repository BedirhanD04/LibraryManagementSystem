namespace LibraryManagementSystem.Entities.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; } // Foreign key to the Books table
        public Book? Book { get; set; } // Navigation property

        public int MemberId { get; set; } // Foreign key to the Members table
        public Member? Member { get; set; } // Navigation property

        public DateTime LoanDate { get; set; } = DateTime.Now; // Default value is the current date and time
        public DateTime DueDate { get; set; } // The date by which the book should be returned
        public DateTime? ReturnDate { get; set; } // Nullable, as the book may not have been returned yet
        public bool IsReturned => ReturnDate != null; // Computed property: NOT a separate database column, automatically derived from ReturnDat
    }   
}