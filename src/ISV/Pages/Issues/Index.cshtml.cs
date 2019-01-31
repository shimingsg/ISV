using ISV.Data;
using ISV.Models;
using ISV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISV.Pages.Issues
{
    public class IndexModel : IVPageModel
    {
        private readonly IGithubService _githubservice;
        public IndexModel(AppDbContext context, IGithubService githubservice, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
        {
            _githubservice = githubservice;
        }

        public IList<Issue> Issue { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedCategoryId { get; set; }

        public List<SelectListItem> SelectingCategories { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchIssueId { get; set; }

        public async Task OnGetAsync()
        {
            var issues = from m in _context.Issues
                         select m;
            if (!string.IsNullOrEmpty(SelectedCategoryId))
            {
                var categoryId = Convert.ToInt32(SelectedCategoryId);
                if (categoryId >= 0)
                {
                    issues = issues.Where(s => s.CategoryId == categoryId);
                }
            }

            if (!string.IsNullOrEmpty(SearchIssueId))
            {
                var searchIssueId = Convert.ToInt32(SearchIssueId);
                if (searchIssueId >= 0)
                {
                    issues = issues.Where(s => s.IssueId == searchIssueId);
                }
            }

            issues = issues.OrderByDescending(s => s.LastUpdatedAt);

            Issue = await issues.Include(i => i.Category).ToListAsync();

            SelectingCategories =
                await _context.Categories.Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.Name
                                                }).ToListAsync();
        }
    }
}
