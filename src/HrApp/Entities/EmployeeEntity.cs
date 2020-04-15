using CodeMash.Models;
using System;
using System.Collections.Generic;

 namespace HrApp
 {
    [Collection("pc-employees")]
   // [Collection("pc-employees-testing")]
    public class EmployeeEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; } = "";

        [Field("last_name")]
        public string LastName { get; set; } = "";

        [Field("division")]
        public string Division { get; set; } = "";

        [Field("role")]
        public int Role { get; set; } = 0;

        [Field("budget")]
        public int Budget { get; set; } = 0;

        [Field("time_worked")]
        public double TimeWorked { get; set; } = 0;
        [Field("address")]
        public string Address { get; set; } = "";

        [Field("employees")]
        public List<string> Employees { get; set; } = new List<string>();

        [Field("application_user")]
        public Guid UserId { get; set; } = new Guid();

        [Field("business_trips")]
        public List<Trip> BussinessTrips { get; set; } = new List<Trip>();

        [Field("personal_identification_number")]
        public string PersonalId { get; set; } = "";

        [Field("manager")]
        public string ManagerId { get; set; } = "";
        [Field("business_email")]
        public string BusinessEmail { get; set; } = "";
        [Field("number_of_holidays_left")]
        public int NumberOfHolidays { get; set; } = 0;
        [Field("pass_no_or_id_card_no")]
        public string PassOrIDCardNo { get; set; } = "";
        [Field("document_expiration_date")]
        public DateTime DocumentExpirationDate { get; set; } = DateTime.Now;
        [Field("iban")]
        public string IBAN { get; set; } = "";
        [Field("photo")]
        public List<object> Photo { get; set; } = new List<object>();
        [Field("emergency_contact")]
        public List<EmergencyContact> Emergency { get; set; } = new List<EmergencyContact>();
        [Field("describe_yourself_in_3_words")]
        public string DesribeYourselfIn3Words { get; set; } = "";
        [Field("funny_fact")]
        public string FunnyFact { get; set; } = "";
        [Field("computer_part")]
        public string ComputerPart { get; set; } = "";
        [Field("hidden_talent")]
        public string HiddenTalent { get; set; } = "";
        [Field("go_anywhere")]
        public string GoAnywhere { get; set; } = "";
        [Field("movie_or_book")]
        public string MovieOrBook { get; set; } = "";
        [Field("spare_time")]
        public string SpareTime { get; set; } = "";
    }

 }

