using Dapper;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BooksController : ControllerBase
    {
        private readonly string _connectionString= "Data Source=(local);Initial Catalog=LibraryManagement;Integrated Security=True;";

        // GET: api/books
        [HttpGet(Name = "GetBooks")]
        public IActionResult GetBooks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var books = connection.Query<Book>("SELECT * FROM Books");
                return Ok(books);
            }
        }

        //GET: api/books/{id
        [HttpGet(Name = "GetBooks/{id}")]
        public IActionResult GetBookByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var book = connection.QueryFirstOrDefault<Book>("SELECT * FROM Books WHERE BookID = @BookID", new { BookID = id });
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
        }

        // POST: api/books
        [HttpPost(Name = "PostBooks")]
        public IActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Books (BookID,Title, Author, Genre, ISBN, PublicationDate, Available) VALUES (@BookID,@Title, @Author, @Genre, @ISBN, @PublicationDate, @Available)";
                connection.Execute(query, book);
            }

            return CreatedAtRoute("DefaultApi", new { id = book.BookID }, book);
        }

        // PUT: api/books/{id}
        [HttpPut(Name = "PutBook/{id}")]
        public IActionResult PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookID)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, ISBN = @ISBN, PublicationDate = @PublicationDate, Available = @Available WHERE BookID = @BookID";
                connection.Execute(query, book);
            }

            return Ok();
        }        
    }
}
