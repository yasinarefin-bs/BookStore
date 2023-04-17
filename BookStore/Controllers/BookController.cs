using BookStore.Models;
using BookStore.Util;
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
        private readonly IWebHostEnvironment _env;

        public BookController(SqlDbContext context, ILogger<BookController> logger, IWebHostEnvironment env)
        {
            _db = context;
            _logger = logger;
            _env = env;
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
           
            return View(new BookViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel obj)
        {

            if (ModelState.IsValid)
            {
                var fileName = ImageUtil.SaveImage(obj.CoverPic, "book", _env);

                var bookObj = new Book()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    Language = obj.Language,
                    AuthorId= obj.AuthorId,
                         
                    CoverPicUrl = $"/image/book/{fileName}"

                };

                _db.Books.Add(bookObj);
                _db.SaveChanges();
                TempData["Success"] = "Book created successfully";
                return RedirectToAction(nameof(Index));
            }

            //var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //{
            //    _logger.LogError(error.ErrorMessage);
            //}
            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;
            return View(obj);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookObject = _db.Books.Include(book=>book.Author).Where(book=>book.Id == id).First();

            if (bookObject == null)
            {
                return NotFound();
            }

            return View(bookObject);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookObject = _db.Books.Find(id);

            if (bookObject == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel()
            {
                Id = bookObject.Id,
                Name = bookObject.Name,
                Description = bookObject.Description,
                AuthorId = bookObject.AuthorId,
                Language = bookObject.Language,

            };

            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;

            return View(bookViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BookViewModel obj)
        {

            if (ModelState.IsValid)
            {
                var fileName = ImageUtil.SaveImage(obj.CoverPic, "book", _env);

                var bookObj = new Book()
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description,
                    Language = obj.Language,
                    AuthorId = obj.AuthorId,

                    CoverPicUrl = $"/image/book/{fileName}"

                };

                _db.Books.Update(bookObj);
                _db.SaveChanges();
                TempData["Success"] = "Book created successfully";
                return RedirectToAction(nameof(Index));
            }

            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookObject = _db.Books.Find(id);

            if (bookObject == null)
            {
                return NotFound();
            }
            _db.Books.Remove(bookObject);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
