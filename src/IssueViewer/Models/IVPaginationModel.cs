using IssueViewer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IssueViewer.Models
{
    public class IVPaginationModel : IVPageModel
    {
        public int PageSize { get; protected set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; protected set; } = 1;
        public int TotalPages { get; set; }
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;
        public int Count { get; set; }
        public IVPaginationModel(AppDbContext context, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
        {

        }

        protected async Task UpdatePageVariablesAsync<T>() where T : IVEntity
        {
            Count = await _context.Set<T>().CountAsync();
            TotalPages = (int)Math.Ceiling(decimal.Divide(Count, PageSize));
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            else if (CurrentPage <= 0) CurrentPage = 1;
        }
    }
}
