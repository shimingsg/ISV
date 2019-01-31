using ISV.Data;
using ISV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ISV.Pages.Categories
{
    public class DetailsModel : IVPageModel
    {
        public DetailsModel(AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config) 
            : base(context, loggerFactory, config)
        {
        }

        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories
                .Include(c => c.Parent).FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
