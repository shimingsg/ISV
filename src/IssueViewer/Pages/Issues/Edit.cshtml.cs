using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueViewer.Data;
using IssueViewer.Models;
using IssueViewer.Services;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages.Issues
{
    public class EditModel : IVPageModel
    {
        public EditModel(AppDbContext context,
            ILoggerFactory loggerFactory) 
            : base(context, loggerFactory)
        {
         
        }

        [BindProperty]
        public Issue Issue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issues
                .Include(i => i.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Issue == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StateId"] = CommonUtilities.GetSelectListFor<State>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Issue.LastUpdatedAt = DateTime.Now;
            _context.Attach(Issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists<Issue>(Issue.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
