using BookStore.Models;
using BookStpre.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {

        private readonly SqlDbContext _db;
        private readonly ILogger<BookController> _logger;

        public BookController(SqlDbContext context, ILogger<BookController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var books = _db.Books.Include(b => b.Author).ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            var authors = _db.Authors.ToList();

            ViewBag.Authors = authors;

            return View(new Book());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book obj)
        {

            ModelState.Remove("Author");

            if (ModelState.IsValid)
            {
                _db.Books.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Book created successfully";
                return RedirectToAction(nameof(Index));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage);
            }
            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;
            return View(obj);
        }
    }
}
