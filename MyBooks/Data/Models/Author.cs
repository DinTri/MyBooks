namespace MyBooks.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        //Navigation properties
        public List<BookAuthor> Book_Authors { get; set; }
    }
}
