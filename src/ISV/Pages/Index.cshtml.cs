using ISV.Data;
using ISV.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ISV.Pages
{
    public class IndexModel : ISV.Models.IVPageModel
    {
        public IndexModel(AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
        {

        }

        public IList<Category> Categories { get; set; }


        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }
    }
}