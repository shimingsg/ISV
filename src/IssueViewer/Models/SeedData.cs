using IssueViewer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(GenerateCategories());
                    context.SaveChanges();
                }

                if (!context.Issues.Any())
                {
                    context.Issues.AddRange(GenerateIssues());
                    context.SaveChanges();
                }
            }
        }

        private static List<Issue> GenerateIssues()
        {
            return new List<Issue>()
            {
                new Issue
                {
                    RepoIdentier="github",
                    Link = "https://github.com/Microsoft/PTVS/issues/4780"
                },
                new Issue
                {
                    RepoIdentier="github",
                    Link = "https://github.com/Microsoft/PTVS/issues/4781"
                },
                new Issue
                {
                    RepoIdentier="github",
                    Link = "https://github.com/Microsoft/PTVS/issues/4784"
                }
            };
        }

        private static List<Category> GenerateCategories()
        {
            return new List<Category>()
            {
                new Category
                {
                    Name = "E2E"
                },
                    new Category
                    {
                        Name = "Unit Test"
                    },
                    new Category
                    {
                        Name = "PTVS"
                    },
                    new Category
                    {
                        Name = "PVSC"
                    },
                    new Category
                    {
                        Name = "ci.dot.net"
                    },
                    new Category
                    {
                        Name = "ci2.dot.net"
                    }
            };


        }
    }
}
