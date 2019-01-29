using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IssueViewer.Data;
using IssueViewer.Models;
using IssueViewer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IssueViewer.Pages.Issues
{
    public class UpdateModel : IssueViewer.Models.IVPageModel
    {
        private readonly IGithubService _githubservice;
        private IHostingEnvironment _env;

        public UpdateModel(AppDbContext context,
            IGithubService githubservice,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
            : base(context, loggerFactory)
        {
            _githubservice = githubservice;
            _env = env;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedCategoryId { get; set; }

        public List<SelectListItem> SelectingCategories { get; set; }

        private async Task GetCategoriesAsync()
        {
            SelectingCategories =
                await _context.Categories.Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.Name
                                                }).ToListAsync();
        }

        public async Task OnGetAsync()
        {
            await GetCategoriesAsync();
        }

        public async Task<IActionResult> OnPostUpdateSelectedAsync()
        {
            _logger.LogInformation("OnPostUpdateSelectedAsync ...");
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
                Octokit.Issue issue = null;
                try
                {
                    issue = await _githubservice.GetGithubIssueAsync(current.Link);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"error when getting info about {current.Link}");
                    _logger.LogError(ex.Message);
                    continue;
                    throw;
                }

                if (issue != null)
                {
                    current.IssueId = issue.Number;
                    current.Title = issue.Title;
                    current.IssueCreatedAt = issue.CreatedAt.DateTime;
                    current.IssueLastUpdatedAt = issue.UpdatedAt?.DateTime;
                    current.IssueClosedAt = issue.ClosedAt?.DateTime;
                    current.State = (State)issue.State.Value;
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

            return RedirectToPage("./Index");
        }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public async Task<IActionResult> OnPostImportAsync()
        {
            if (FileUpload == null)
            {
                this.ModelState.AddModelError("", "OnPostImportAsync test error");
                await GetCategoriesAsync();
                return Page();
            }
            using (var reader = new StreamReader(FileUpload.OpenReadStream()))
            {
                var issuesFromCSV = CommonUtilities.ReadCSV(reader);

                if (issuesFromCSV.Count() <= 0)
                {
                    ModelState.AddModelError("", "no data in file");
                    await GetCategoriesAsync();
                    return Page();
                }

                var issues = from m in _context.Issues
                             select m;
                var issuesInDB = await issues.ToListAsync();
                var except = issuesFromCSV.Except(issuesInDB, new IssueComparer());
                if (except.Count() > 0)
                {
                    foreach (var i in except)
                    {
                        i.CreatedAt = DateTime.Now;
                        i.LastUpdatedAt = i.CreatedAt;
                        _context.Issues.Add(i);
                    }

                    await _context.SaveChangesAsync();

                    this.ModelState.AddModelError("", $"{except.Count()} records imported from csv file.");
                }
                else
                {
                    this.ModelState.AddModelError("", "No data in csv file.");
                }
                //this.ModelState.AddModelError("", result);

                //return RedirectToPage("./Issues/Index");


            }

            await GetCategoriesAsync();
            return Page();
            //_env.ContentRootPath //Application Base Path
            //_env.WebRootPath //wwwroot folder path
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            var issues = from m in _context.Issues
                         select m;
            issues = issues.Where(c => c.RepoIdentier.ToLower() == "github");

            var all = await issues.ToListAsync();
            var fileName = $"exported_{DateTime.Now.ToString("yyyyMMddhhmmss")}.csv";
            var path = Path.Combine(_env.WebRootPath, "output", fileName);
            try
            {
                CommonUtilities.WriteCSV(all, path);
                return File($"/output/{fileName}", "application/octet-stream", fileName);
                //return File(stream, "application/octet-stream", "Reports.csv");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            await GetCategoriesAsync();
            return Page();
        }
    }

    public class IssueComparer : IEqualityComparer<Issue>
    {
        public bool Equals(Issue x, Issue y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null)
                || Object.ReferenceEquals(y, null))
                return false;

            return x.Link.Trim().ToLower() == y.Link.Trim().ToLower();
        }

        public int GetHashCode(Issue issue)
        {
            if (Object.ReferenceEquals(issue, null)) return 0;

            int hashIssueLink = issue.Link == null ? 0 : issue.Link.GetHashCode();

            return hashIssueLink;
        }
    }

}
