using IssueViewer.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class IVPageModel : PageModel
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger _logger;

        public IVPageModel(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().ToString());
        }

        protected bool EntityExists<T>(int id) where T : IVEntity
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public override Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context, 
            PageHandlerExecutionDelegate next)
        {
            _logger.LogDebug($"OnPageHandlerExecutionAsync {this.GetType().ToString()}");
            return base.OnPageHandlerExecutionAsync(context, next);
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _logger.LogDebug($"OnPageHandlerExecuting {this.GetType().ToString()}");
            base.OnPageHandlerExecuting(context);
        }

    }
}
