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

namespace IssueViewer.Pages.Categories
{
    public class IndexModel : IVPageModel
    {
        public IndexModel(IssueViewer.Data.AppDbContext context,
            ILoggerFactory loggerFactory) : base(context,loggerFactory)
        {

        }

        public IList<Category> Category { get; set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categories
                .Include(c => c.Parent).ToListAsync();
        }
    }
}
