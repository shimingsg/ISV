using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IssueViewer.Data;
using IssueViewer.Models;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages.Categories
{
    public class CreateModel : IVPageModel
    {
        public CreateModel(AppDbContext context,
            ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public IActionResult OnGet()
        {
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Category.CreatedAt = DateTime.Now;
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}