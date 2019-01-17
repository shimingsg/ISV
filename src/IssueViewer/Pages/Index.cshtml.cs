using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueViewer.Data;
using IssueViewer.Models;
using IssueViewer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IssueViewer.Pages
{
    public class IndexModel : IssueViewer.Models.IVPageModel
    {
        private readonly IGithubService _githubservice;

        public IndexModel(AppDbContext context, IGithubService githubservice) : base(context)
        {
            _githubservice = githubservice;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedCategoryId { get; set; }

        public List<SelectListItem> SelectingCategories { get; set; }


        public async Task OnGetAsync()
        {
            SelectingCategories =
                await _context.Categories.Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.Name
                                                }).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var issues = from m in _context.Issues
                         select m;

            issues = issues.Where(c => c.RepoIdentier.ToLower() == "github");

            if (!string.IsNullOrEmpty(SelectedCategoryId))
            {
                var categoryId = Convert.ToInt32(SelectedCategoryId);
                if (categoryId >= 0)
                {
                    issues = issues.Where(s => s.CategoryId == categoryId);
                }
            }

            var all = await issues.ToListAsync();

            foreach (var current in all)
            {
                var issue = await _githubservice.GetGithubIssueAsync(current.Link);
                if (issue != null)
                {
                    current.IssueId = issue.Number;
                    current.Title = issue.Title;
                    current.IssueCreatedAt = issue.CreatedAt.DateTime;
                    current.IssueLastUpdatedAt = issue.UpdatedAt?.DateTime;
                    current.IssueClosedAt = issue.ClosedAt?.DateTime;
                    current.State = (int?)issue.State.Value;
                    _context.Attach(current).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EntityExists<Issue>(current.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            return RedirectToPage("./Issues/Index");
        }
    }
}
