using System;
using CodeMash.Models;

namespace HrApp.Entities
{
    [CollectionName("Commits")]
    public class CommitEntity : Entity
    {
        [FieldName("description")]
        public string Description { get; set; }

        [FieldName("commit_date")]
        public DateTime CommitDate { get; set; }

        [FieldName("time_worked")]
        public TimeSpan TimeWorked { get; set; }

        [FieldName("employee")]
        public string Employee { get; set; }
    }
}
