using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Issue
    {
        public int Id { get; set; }

        //[Required]
        [Display(Name = "Repo Identier")]
        //[StringLength(75)]
        public string RepoIdentier { get; set; }

        public int IssueId { get; set; }

        public string Title { get; set; }

        [NotMapped]
        public string ShortTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Title) && this.Title.Length > 60)
                    return this.Title.Substring(0, 57) + "...";
                return this.Title;
            }
        }

        [Display(Name = "Created")]
        public DateTime? CreatedDateTime { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdatedDateTime { get; set; }

        [Display(Name = "Closed")]
        public DateTime? ClosedDateTime { get; set; }

        public int? State { get; set; }

        public string Link { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
