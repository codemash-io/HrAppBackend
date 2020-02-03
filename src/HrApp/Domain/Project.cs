using HrApp.Entities;
using System;
using System.Collections.Generic;

namespace HrApp.Domain
{
    public class Project
    {
        public Project(string name, string description, double budget, List<EmployeeEntity> employees)
        {
            Name = name;
            Description = description;
            Budget = budget;
            Employees = employees;
            DateCreated = DateTime.Now;
            Commits = new List<CommitEntity>(); //when creating project there is no commits yet
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Budget { get; set; }            //project budget in hours
        public DateTime DateCreated { get; set; }
        public List<EmployeeEntity> Employees { get; set; }
        public List<CommitEntity> Commits { get; set; }
    }
}
