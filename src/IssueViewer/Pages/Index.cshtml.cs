using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IssueViewer.Pages
{
    public class IndexModel : IssueViewer.Models.IVPageModel
    {
        public IndexModel(IssueViewer.Data.AppDbContext context)
            : base(context)
        {

        }

        public IList<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }
    }
}