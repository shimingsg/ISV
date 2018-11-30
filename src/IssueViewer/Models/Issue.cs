using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Issue
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// github/devdiv
        /// </summary>
        public string RepoId { get; set; }
        public int IssueId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public DateTime ClosedDateTime { get; set; }
        public int State { get; set; }
        public string Link { get; set; }
        /// <summary>
        /// category
        /// </summary>
        public string Category { get; set; }
    }
}
