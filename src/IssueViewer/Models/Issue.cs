﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string RepoId { get; set; }
        public int IssueId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string State { get; set; }
        public string Link { get; set; }
    }
}