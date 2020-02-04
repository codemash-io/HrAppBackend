using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp.Domain;

namespace HrApp.Entities
{
    public class EmployeeInfoDto
    {
        public List<PhoneEntity> Phones { get; set; }
        public List<ComputerEntity> Computers { get; set; }
        public List<MonitorEntity> Monitors { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Cash> Funds { get; set; }
        public CompetencyLevelMeta CompetencyLevelMeta { get; set; }

    }
}
