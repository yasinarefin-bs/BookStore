using BookStore.Models;
using BookStpre.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BookStore.Util;
using System.Net;
using System.Xml.Linq;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly SqlDbContext _db;
        private readonly ILogger<BookController> _logger;
        private readonly IWebHostEnvironment _env;


        public AuthorController(SqlDbContext context, ILogger<BookController> logger, IWebHostEnvironment env) 
        {
            _db = context;
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            var authorList = _db.Authors.ToList();
            return View(authorList);
        }
        public IActionResult Details(int? id)
        { 
            if(id == null)
            {
                return NotFound();
            }

            var authorObject = _db.Authors.FirstOrDefault(x => x.Id == id);

            if(authorObject == null)
            {
                return NotFound();
            }

            var viewModel = new AuthorDetailsViewModel() {
                Author = authorObject,
                ListOfBooks = _db.Books.Where(book => book.AuthorId==id).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorObject = _db.Authors.FirstOrDefault(x => x.Id == id);
            if (authorObject == null)
            {
                return NotFound();
            }

            var authorViewModel = new AuthorViewModel() {
                Name = authorObject.Name,
                Address = authorObject.Address,
                Email = authorObject.Email,
            };
            
            return View(authorViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AuthorViewModel obj)
        {
            if (ModelState.IsValid)
            {
                // saves and returns unique filename
                var fileName = ImageUtil.SaveImage(obj.AuthorPicture, "author", _env);


                var authorObj = new Author() 
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Email = obj.Email,
                    Address = obj.Address,
                    ImageUrl = $"/image/author/{fileName}"

                };

                _db.Authors.Update(authorObj);
                _db.SaveChanges();
                TempData["Success"] = "Author created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var authorObj = _db.Authors.Find(id);
            if(authorObj == null)
            {
                return NotFound();
            }
            _db.Authors.Remove(authorObj);
            _db.SaveChanges();

            ImageUtil.DeleteImage(authorObj.ImageUrl, _env, _logger);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorViewModel obj)
        {
            if (ModelState.IsValid)
            {
                // saves and returns unique filename
                var fileName = ImageUtil.SaveImage(obj.AuthorPicture, "author",  _env);


                var authorObj = new Author()
                {
                    Name = obj.Name,
                    Email = obj.Email,
                    Address = obj.Address,
                    ImageUrl = $"/image/author/{fileName}"

                };

                _db.Authors.Add(authorObj);
                _db.SaveChanges();
                TempData["Success"] = "Author created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

    }
}
