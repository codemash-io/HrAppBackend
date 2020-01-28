using System;
using CodeMash.Models;

namespace HrApp.Entities
{
    [Collection("Commits")]
    public class CommitEntity : Entity
    {
        [Field("description")]
        public string Description { get; set; }

        [Field("commit_date")]
        public DateTime CommitDate { get; set; }

        [Field("time_worked")]
        public double TimeWorked { get; set; } // in hours

        [Field("employee")]
        public string Employee { get; set; }
    }
}
