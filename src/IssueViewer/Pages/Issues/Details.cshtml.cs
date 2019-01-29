using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IssueViewer.Data;
using IssueViewer.Models;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages.Issues
{
    public class DetailsModel : IVPageModel
    {
        public DetailsModel(IssueViewer.Data.AppDbContext context,
            ILoggerFactory loggerFactory)
            : base(context, loggerFactory)
        {
       
        }

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
            return Page();
        }
    }
}
