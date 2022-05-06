namespace MyBooks.Data.ViewModels
{
    public class PublisherVm
    {
        public string Name { get; set; }
    }
    public class PublisherWithBookAndAuthorVm
    {
        public string Name { get; set; }
        public List<BookAuthorVm> BookAuthors { get; set; }
    }

    public class BookAuthorVm
    {
        public string BookName { get; set; }
        public List<string> BookAuthors { get; set; }
    }
}
