using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp.Domain;

namespace HrApp.Entities
{
    public class ComputerEntity : CustomEntity
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string CPU { get; set; }
        public int RAM { get; set; }
        public List<Hdd> HDD { get; set; }
        public List<Note> Notes { get; set; }
    }
}
