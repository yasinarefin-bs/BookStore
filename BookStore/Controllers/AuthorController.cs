﻿using BookStore.Models;
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
            return View(authorObject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Author obj)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Update(obj);
                _db.SaveChanges();

                TempData["Success"] = "Author updated successfully.";
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

            return RedirectToAction(nameof(Index));
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
