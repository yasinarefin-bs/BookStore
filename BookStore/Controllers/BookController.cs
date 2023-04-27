using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Models.DbModels;
using BookStore.Util;
using BookStpre.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using BookStore.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {

        private readonly SqlDbContext _db;
        private readonly ILogger<BookController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BookValidationService _bookValService;

        public BookController(SqlDbContext context, ILogger<BookController> logger, IWebHostEnvironment env, BookValidationService bookValService)
        {
            _db = context;
            _logger = logger;
            _env = env;
            _bookValService = bookValService;
        }
        public IActionResult Index([FromQuery(Name ="search")] string searchKeyword)
        {
            
            if(searchKeyword != null)
            {
                var books =  _db.Books.Include(b => b.Author).Where(b => b.Name.Contains(searchKeyword)).ToList();
                return View(books);
            }
            else
            {
                var books = _db.Books.Include(b => b.Author).ToList();
                return View(books);
            }
            
            
        }

        public IActionResult Create()
        {
            var authors = _db.Authors.ToList();

            ViewBag.Authors = authors;
           
            return View(new BookViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel bookViewModel)
        {
            if (_bookValService.IsValid(bookViewModel, ModelState))
            {
                var fileName = ImageUtil.SaveImage(bookViewModel.CoverPic, "book", _env);

                var bookObj = new Book()
                {
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    Language = bookViewModel.Language,
                    AuthorId = bookViewModel.AuthorId,

                    CoverPicUrl = $"/image/book/{fileName}"

                };
                _db.Books.Add(bookObj);
                _db.SaveChanges();
                TempData["Success"] = "Book created successfully";
                return RedirectToAction(nameof(Index));
            }
            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;


            return View(bookViewModel);


        }

            
        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(BookViewModel bookViewModel)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        var fileName = ImageUtil.SaveImage(bookViewModel.CoverPic, "book", _env);

        //        var bookObj = new Book()
        //        {
        //            Name = bookViewModel.Name,
        //            Description = bookViewModel.Description,
        //            Language = bookViewModel.Language,
        //            AuthorId = bookViewModel.AuthorId,

        //            CoverPicUrl = $"/image/book/{fileName}"

        //        };

        //        _db.Books.Add(bookObj);
        //        _db.SaveChanges();
        //        TempData["Success"] = "Book created successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    _logger.LogInformation("RIPPP");

        //    var authors = _db.Authors.ToList();
        //    ViewBag.Authors = authors;
        //    return View(bookViewModel);
        //}

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
        public IActionResult Update(BookViewModel bookViewModel)
        {

            if (ModelState.IsValid)
            {
                var fileName = ImageUtil.SaveImage(bookViewModel.CoverPic, "book", _env);

                var bookObj = new Book()
                {
                    Id = bookViewModel.Id,
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    Language = bookViewModel.Language,
                    AuthorId = bookViewModel.AuthorId,

                    CoverPicUrl = $"/image/book/{fileName}"

                };

                _db.Books.Update(bookObj);
                _db.SaveChanges();
                TempData["Success"] = "Book created successfully";
                return RedirectToAction(nameof(Index));
            }

            var authors = _db.Authors.ToList();
            ViewBag.Authors = authors;

            return View(bookViewModel);
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
