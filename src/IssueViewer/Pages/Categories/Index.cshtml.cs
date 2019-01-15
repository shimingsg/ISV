using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IssueViewer.Data;
using IssueViewer.Models;

namespace IssueViewer.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IssueViewer.Data.AppDbContext _context;

        public IndexModel(IssueViewer.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categories
                .Include(c => c.Parent).ToListAsync();
        }
    }
}
