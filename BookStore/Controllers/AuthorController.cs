using BookStore.Models;
using BookStpre.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly SqlDbContext _db;
        private readonly ILogger<BookController> _logger;

        public AuthorController(SqlDbContext context, ILogger<BookController> logger) 
        {
            _db = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var authorList = _db.Authors.ToList();
            return View(authorList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author obj)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Author created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
