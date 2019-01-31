using ISV.Data;
using ISV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISV.Pages.Categories
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
