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
    public class IndexModel : IVPaginationModel
    {
        private readonly IGithubService _githubservice;
        public IndexModel(AppDbContext context, IGithubService githubservice, ILoggerFactory loggerFactory, IConfiguration config)
            : base(context, loggerFactory, config)
        {
            _githubservice = githubservice;
            Title = "Issues - List";
        }

        public IList<Issue> Issue { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedCategoryId { get; set; }

        public List<SelectListItem> SelectingCategories { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchIssueId { get; set; }

        public async Task OnGetAsync(int? currentPage)
        {
            var issues = from m in _context.Issues
                         select m;
            if (!string.IsNullOrEmpty(SelectedCategoryId))
            {
                var categoryId = Convert.ToInt32(SelectedCategoryId);

                if (categoryId >= 0)
                {
                    var children = await GetCategoryChildren(categoryId);
                    if (children.Count == 0)
                        issues = issues.Where(s => s.CategoryId == categoryId);
                    else
                    {
                        issues = issues.Where(x => children.Any(s => s.Id == x.CategoryId) || x.CategoryId == categoryId);
                    }
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

            CurrentPage = currentPage ?? 1;
            await UpdatePageVariablesAsync<Issue>();
            issues = issues.OrderByDescending(s => s.LastUpdatedAt)
                            .Skip((CurrentPage - 1) * PageSize)
                            .Take(PageSize);
            Issue = await issues.Include(i => i.Category).ToListAsync();

            SelectingCategories =
                await _context.Categories.Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.Name
                                                }).ToListAsync();
        }

        private async Task<List<Category>> GetCategoryChildren(int id)
        {
            return await _context.Categories.Where(t => t.ParentId == id).ToListAsync();
        }
    }
}
