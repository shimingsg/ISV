using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IssueViewer.Data;
using IssueViewer.Models;
using IssueViewer.Services;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages.Issues
{
    public class CreateModel : IVPageModel
    {
        public CreateModel(AppDbContext context, 
            ILoggerFactory loggerFactory)
            :base(context,loggerFactory)
        {
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StateId"] = CommonUtilities.GetSelectListFor<State>();
            return Page();
        }

        [BindProperty]
        public Issue Issue { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Issue.CreatedAt = DateTime.Now;
            Issue.LastUpdatedAt = Issue.CreatedAt;
            _context.Issues.Add(Issue);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}