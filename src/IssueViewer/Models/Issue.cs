using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Issue : IVEntity
    {
        [Required]
        [Display(Name = "Repo")]
        //[StringLength(75)]
        public string RepoIdentier { get; set; }

        public int? IssueId { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd ss:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime? IssueCreatedAt { get; set; }

        [Display(Name = "Updated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd ss:mm:ss}")]
        public DateTime? IssueLastUpdatedAt { get; set; }

        [Display(Name = "Closed")]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd ss:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime? IssueClosedAt { get; set; }

        public int? State { get; set; }

        public string Link { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
