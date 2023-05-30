using Dapper;
using Library.Context;
using Library.Models;
using System.Collections.Generic;

namespace Library.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
    }
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BookRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Book> GetAll()
        {
            var query = "Select * from Books";
            using (var connection = _dbContext.CreateConnection())
            {
                var books = connection.Query<Book>(query);
                return books;
            }
        }
    }
}