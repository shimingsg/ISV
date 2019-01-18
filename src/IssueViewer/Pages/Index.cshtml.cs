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

namespace IssueViewer.Pages
{
    public class IndexModel : IssueViewer.Models.IVPageModel
    {
        private readonly IGithubService _githubservice;
        private IHostingEnvironment _env;

        public IndexModel(AppDbContext context,
            IGithubService githubservice,
            IHostingEnvironment env)
            : base(context)
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
                var result = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    this.ModelState.AddModelError("", result);
                    await GetCategoriesAsync();
                    return Page();
                    //return RedirectToPage("./Issues/Index");
                }

                ModelState.AddModelError("", "no content in file");
                await GetCategoriesAsync();
                return Page();
            }
            //_env.ContentRootPath //Application Base Path
            //_env.WebRootPath //wwwroot folder path
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            this.ModelState.AddModelError("", "OnExportAsync test error");
            await GetCategoriesAsync();
            return Page();
        }
    }
}
