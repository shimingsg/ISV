using ISV.Data;
using ISV.Models;
using ISV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ISV.Pages.Issues
{
    public class CreateModel : IVPageModel
    {
        public CreateModel(AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
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