using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueViewer.Data;
using IssueViewer.Models;
using IssueViewer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var issues = from m in _context.Issues
                         select m;

            issues = issues.Where(c => c.RepoIdentier.ToLower() == "github");

            var all = await issues.Include(i => i.Category).ToListAsync();

            foreach (var current in all)
            {
                var issue = await _githubservice.GetGithubIssueAsync(current.Link);
                if (issue != null)
                {
                    current.Title = issue.Title;
                    current.CreatedDateTime = issue.CreatedAt.DateTime;
                    current.LastUpdatedDateTime = issue.UpdatedAt?.DateTime;
                    current.ClosedDateTime = issue.ClosedAt?.DateTime;
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
