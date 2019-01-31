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
using Microsoft.Extensions.Configuration;

namespace IssueViewer.Pages.Categories
{
    public class IndexModel : IVPaginationModel
    {
        public IndexModel(AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory,config)
        {
            //PageSize = config.GetValue()
        }

        public IList<Category> Category { get; set; }

        public async Task OnGetAsync(int? currentPage)
        {
            CurrentPage = currentPage ?? 1;
            await UpdatePageVariablesAsync<Category>();
            Category = await _context.Categories
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .Include(t => t.Parent)
                .ToListAsync();
        }
    }
}
