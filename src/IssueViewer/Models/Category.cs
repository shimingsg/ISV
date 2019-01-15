using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Category: IVEntity
    {
        [Display(Name = "Parent")]
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
