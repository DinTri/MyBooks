using Microsoft.EntityFrameworkCore;
using MyBooks.Data;
using MyBooks.Data.Models;
using MyBooks.Data.Services;
using MyBooks.Data.ViewModels;
using MyBooks.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBooksTest
{
    public class PublisherServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext appDbContext;
        PublishersService publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            appDbContext = new AppDbContext(dbContextOptions);
            appDbContext.Database.EnsureCreated();

            SeedDatabase();
            publishersService = new PublishersService(appDbContext);

        }
        [Test, Order(1)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
        }
        [Test, Order(2)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "Trifon", null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher Trifon"));
        }

        [Test, Order(4)]
        public void GetAllPublishers_WithSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("name_desc", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher Veli"));
        }

        [Test, Order(5)]
        public void GetPublisherById_WithResponse_Test()
        {
            var result = publishersService.GetPublisherById(1);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Publisher Veli"));
        }

        [Test, Order(6)]
        public void GetPublisherById_WithoutResponse_Test()
        {
            var result = publishersService.GetPublisherById(99);

            Assert.That(result, Is.Null);
        }

        [Test, Order(7)]
        public void AddPublisher_WithException_Test()
        {
            var newPublisher = new PublisherVm()
            {
                Name = "123 With Exception"
            };

            Assert.That(() => publishersService.AddPublisher(newPublisher), Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Starts with number"));
        }

        [Test, Order(8)]
        public void AddPublisher_WithoutException_Test()
        {
            var newPublisher = new PublisherVm()
            {
                Name = "Without Exception"
            };

            var result = publishersService.AddPublisher(newPublisher);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("Without"));
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test, Order(9)]
        public void GetPublisherData_Test()
        {
            var result = publishersService.GetPublisherData(1);

            Assert.That(result.Name, Is.EqualTo("Publisher Veli"));
            Assert.That(result.BookAuthors, Is.Not.Empty);
            Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));

            var firstBookName = result.BookAuthors.OrderBy(n => n.BookName).FirstOrDefault().BookName;
            Assert.That(firstBookName, Is.EqualTo("Book 1 Title"));
        }

        [Test, Order(10)]
        public void DeletePublisherById_PublisherExists_Test()
        {
            int publisherId = 6;

            var publisherBefore = publishersService.GetPublisherById(publisherId);
            Assert.That(publisherBefore, Is.Not.Null);
            Assert.That(publisherBefore.Name, Is.EqualTo("Publisher LinTri"));

            publishersService.DeletePbulisherById(publisherId);

            var publisherAfter = publishersService.GetPublisherById(publisherId);
            Assert.That(publisherAfter, Is.Null);
        }

        //[Test, Order(11)]
        //public void DeletePublisherById_PublisherDoesNotExists_Test()
        //{
        //    int publisherId = 7;

        //    Assert.That(() => publishersService.DeletePbulisherById(publisherId), Throws.Exception.TypeOf<Exception>().With.Message.EqualTo($"The publisher with id: {publisherId} does not exist"));
        //}
        [OneTimeTearDown]
        public void CleanUp()
        {
            appDbContext.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                    new Publisher() {
                        Id = 1,
                        Name = "Publisher Veli"
                    },
                    new Publisher() {
                        Id = 2,
                        Name = "Publisher Lina"
                    },
                    new Publisher() {
                        Id = 3,
                        Name = "Publisher Trifon"
                    },
                    new Publisher() {
                        Id = 4,
                        Name = "Publisher Melissa"
                    },
                    new Publisher() {
                        Id = 5,
                        Name = "Publisher TriLin"
                    },
                    new Publisher() {
                        Id = 6,
                        Name = "Publisher LinTri"
                    },
            };
            appDbContext.Publishers.AddRange(publishers);

            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    FullName = "Trifon Dinev"
                },
                new Author()
                {
                    Id = 2,
                    FullName = "Lina Ignatoff"
                }
            };
            appDbContext.Authors.AddRange(authors);


            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                }
            };
            appDbContext.Books.AddRange(books);

            var books_authors = new List<BookAuthor>()
            {
                new BookAuthor()
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                },
                new BookAuthor()
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                },
                new BookAuthor()
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2
                },
            };
            appDbContext.Book_Authors.AddRange(books_authors);


            appDbContext.SaveChanges();
        }
    }
}