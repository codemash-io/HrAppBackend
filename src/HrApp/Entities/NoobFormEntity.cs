using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("n00b-forms")]
    public class NoobFormEntity:Entity
    {
        [Field("_meta")]
        public Meta Meta { get; set; }
        [Field("id")]
        public string PersonalId { get; set; }
        [Field("address")]
        public string Address { get; set; }
        [Field("pass_no_or_id_card_no")]
        public string PassOrIDCardNo { get; set; }
        [Field("document_expiration_date")]
        public DateTime DocumentExpirationDate { get; set; }
        [Field("iban")]
        public string IBAN { get; set; }
        [Field("photo")]
        public List<object> Photo { get; set; } = new List<object>();
        [Field("main_contact_name")]
        public string EmergencyContactName { get; set; }
        [Field("relations_with_the_contact")]
        public string RelationsWithContact { get; set; }
        [Field("emergency_phone")]
        public string EmergencyPhone { get; set; }
        [Field("describe_yourself_in_3_words")]
        public string DesribeYourselfIn3Words { get; set; }
        [Field("funny_fact")]
        public string FunnyFact { get; set; }
        [Field("computer_part")]
        public string ComputerPart { get; set; }
        [Field("hidden_talent")]
        public string HiddenTalent { get; set; }
        [Field("go_anywhere")]
        public string GoAnywhere { get; set; }
        [Field("movie_or_book")]
        public string MovieOrBook { get; set; }
        [Field("spare_time")]
        public string SpareTime { get; set; }
       
    }

    public class Meta
    {
        [Field("responsibleUserId")]
        public string ResponsibleUser { get; set; }

    }
}
