namespace MyBooks.Data.ViewModels
{
    public class AuthorVm
    {
        public string? FullName { get; set; }
    }

    public class AuthorWithBooksVm
    {
        public string? FullName { get; set; }
        public List<string>? BookTitles { get; set; }
    }
}
