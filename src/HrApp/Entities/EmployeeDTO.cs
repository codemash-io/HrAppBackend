using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp.Domain;

namespace HrApp.Entities
{
    public class EmployeeDTO
    {
        public string CompetencyLevel { get; set; }
        public List<PhoneEntity> Phones { get; set; }
        public List<ComputerEntity> Computers { get; set; }
        public List<MonitorEntity> Monitors { get; set; }
        public List<TripDTO> Trips { get; set; }
        public List<CashDTO> Trainings { get; set; }
        public List<CashDTO> CashPurchases { get; set; }
        public Price TrainingsCash { get; set; }
        public Price TrainingsCashLeft { get; set; }
        public Price BudgetFund { get; set; }
        public Price BudgetFundLeft { get; set; }
    }
}
