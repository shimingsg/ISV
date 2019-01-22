using CsvHelper;
using CsvHelper.Configuration;
using IssueViewer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IssueViewer.Services
{
    public class CommonUtilities
    {
        public static List<Issue> ReadCSV(StreamReader reader)
        {
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<IssueMap>();
                return csv.GetRecords<Issue>().ToList();
            }
        }
        public static IEnumerable<Issue> ReadCSV(string csvFilePath)
        {
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<Issue>();
            }

        }

        public static void WriteCSV(IEnumerable<Issue> records, string csvFilePath)
        {
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<IssueMap>();
                csv.WriteRecords(records);
            }
        }

        public sealed class IssueMap : ClassMap<Issue>
        {
            public IssueMap()
            {
                Map(m => m.RepoIdentier);
                Map(m => m.Link);
                Map(m => m.CategoryId);
            }

        }

        public static SelectList GetSelectListFor<T>() where T : struct
        {
            var t = typeof(T);
            if (!t.IsEnum)
            {
                return null;
            }

            var values = Enum.GetValues(typeof(T)).Cast<T>()
                   .Select(e => new { Id = Convert.ToInt32(e), Name = Enum.GetName(t,e) });

            return new SelectList(values, "Id", "Name");
        }
    }
}

