using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueViewer.Data;
using IssueViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages
{
    public class IndexModel : IssueViewer.Models.IVPageModel
    {
        public IndexModel(AppDbContext context,
            ILoggerFactory loggerFactory)
            : base(context, loggerFactory)
        {

        }

        public IList<Category> Categories { get; set; }

       
        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }
    }
}