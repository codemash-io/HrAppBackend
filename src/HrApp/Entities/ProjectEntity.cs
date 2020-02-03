using CodeMash.Models;
using System;
using System.Collections.Generic;

namespace HrApp.Entities
{
    [Collection("Projects")]
    public class ProjectEntity : Entity
    {
        [Field("name")]
        public string Name { get; set; }

        [Field("description")]
        public string Description { get; set; }

        [Field("budget")]
        public double Budget { get; set; }            //project budget in hours

        [Field("date_created")]
        public DateTime DateCreated { get; set; }

        [Field("employees")]
        public List<string> Employees { get; set; }

        [Field("commits")]
        public List<string> Commits { get; set; }
    }
}
