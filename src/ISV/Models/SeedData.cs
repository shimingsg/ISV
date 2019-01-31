using ISV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISV.Models
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
            }
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
                    Name = "Python Tool"
                },
                new Category
                {
                    Name = "CI Jenkins"
                },
                new Category
                {
                    Name = "Core Perf"
                }
            };


        }
    }
}
