using IssueViewer.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class IVPageModel : PageModel
    {
        protected readonly AppDbContext _context;

        public IVPageModel(AppDbContext context)
        {
            _context = context;
        }

        protected bool EntityExists<T>(int id) where T : IVEntity
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }
    }
}
