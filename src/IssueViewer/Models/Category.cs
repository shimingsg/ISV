using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Category: IVEntity
    {

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
