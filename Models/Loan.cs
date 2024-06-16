namespace LibraryManagement.Models
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int BookID { get; set; }
        public int MemberID { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
