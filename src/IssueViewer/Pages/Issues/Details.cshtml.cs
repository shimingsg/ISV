﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IssueViewer.Data;
using IssueViewer.Models;

namespace IssueViewer.Pages.Issues
{
    public class DetailsModel : PageModel
    {
        private readonly IssueViewer.Data.AppDbContext _context;

        public DetailsModel(IssueViewer.Data.AppDbContext context)
        {
            _context = context;
        }

        public Issue Issue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issues
                .Include(i => i.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Issue == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
