using ISV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ISV.Pages.Issues
{
    public class DeleteModel : IVPageModel
    {
        public DeleteModel(ISV.Data.AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issues.FindAsync(id);

            if (Issue != null)
            {
                _context.Issues.Remove(Issue);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
