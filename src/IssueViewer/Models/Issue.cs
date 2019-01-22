using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime? IssueCreatedAt { get; set; }

        [Display(Name = "Updated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime? IssueLastUpdatedAt { get; set; }

        [Display(Name = "Closed")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime? IssueClosedAt { get; set; }

        [DefaultValue(State.Unknown)]
        public State State { get; set; }

        public string Link { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public enum State
    {
        [Display(Name = "Unknown")]
        Unknown = -1,
        [Display(Name = "Open/Active")]
        Open = 0,
        [Display(Name = "Closed")]
        Closed = 1
    }
}
