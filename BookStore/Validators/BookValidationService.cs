using BookStore.Controllers;
using BookStore.Models.DbModels;
using BookStore.Models.ViewModels;
using BookStpre.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace BookStore.Validators
{
    public class BookValidationService
    {
        private readonly SqlDbContext _context;
        private readonly ILogger<BookController> _logger;


        public BookValidationService(SqlDbContext context, ILogger<BookController> logger)
        {
            _context = context;
            _logger = logger;
        }

       
        public bool IsValid(BookViewModel book, ModelStateDictionary modelState)
        {

            if(!modelState.IsValid)
            {
                return false;
            }
            bool isUnique = !_context.Books.Any(b => b.Id != book.Id && b.Name == book.Name);
            if (!isUnique)
            {
                modelState.AddModelError(nameof(book.Name), "Username is already taken");
                return false;
            }
            return true;
        }
    }
}
