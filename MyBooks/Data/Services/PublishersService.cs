using MyBooks.Data.Models;
using MyBooks.Data.Paging;
using MyBooks.Data.ViewModels;
using MyBooks.Exceptions;
using System.Text.RegularExpressions;

namespace MyBooks.Data.Services
{
    public class PublishersService
    {
        private readonly AppDbContext _context;

        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public List<Publisher> GetAllPublishers(string sortBy, string searchString, int? pageNumber)
        {
            var _allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        _allPublishers = _allPublishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                _allPublishers = _allPublishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            //paginating
            int pageSize = 5;
            _allPublishers = PaginatedList<Publisher>.Create(_allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);
            return _allPublishers;
        }

        public Publisher AddPublisher(PublisherVm publisher)
        {
            if (StringStartswithaDigit(publisher.Name)) throw new PublisherNameException("Starts with number", publisher.Name);
            var _publisher = new Publisher()
            {
                Name = publisher.Name,
            };
            _context.Publishers.Add(_publisher);
            _context?.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(x => x.Id == id);
        public PublisherWithBookAndAuthorVm GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBookAndAuthorVm()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVm()
                    {
                        BookName = n.Title,
                        BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();
            return _publisherData;
        }
        public void DeletePbulisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);
            if (_publisher != null) { _context.Publishers.Remove(_publisher); _context.SaveChanges(); }
            else
            {
                throw new Exception(message: $"The publisher with id: {id} does not exists");
            }
        }
        private static bool StringStartswithaDigit(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            return Regex.IsMatch(name, @"^\d");
        }
    }
}
