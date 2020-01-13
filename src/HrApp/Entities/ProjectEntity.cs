using CodeMash.Models;
using System;
using System.Collections.Generic;

namespace HrApp.Entities
{
    [CollectionName("Commits")]
    public class ProjectEntity : Entity
    {
        [FieldName("name")]
        public string Name { get; set; }

        [FieldName("description")]
        public string Description { get; set; }

        [FieldName("project_budget")]
        public double Budget { get; set; }            //project budget in hours

        [FieldName("date_created")]
        public DateTime DateCreated { get; set; }

        [FieldName("employees")]
        public List<string> Employees { get; set; }

        [FieldName("commits")]
        public List<string> Commits { get; set; }
    }
}
