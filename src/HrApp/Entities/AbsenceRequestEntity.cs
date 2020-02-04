using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("absence-requests")]
    public class AbsenceRequestEntity:Entity
    {
        [Field("employee")]
        public string Employee { get; set; }
        [Field("type")]
        public string Type { get; set; }
        [Field("start")]
        public DateTime AbsenceStart { get; set; }
        [Field("end")]
        public DateTime AbsenceEnd { get; set; }
        [Field("status")]
        public string Status { get; set; } = AbsenceRequestStatus.Pending.ToString();
        [Field("should_be_approved_by")]
        public List<Guid> ShouldBeApprovedBy { get; set; } = new List<Guid>();
        [Field("approved_by")]
        public List<Guid> ApprovedBy { get; set; } = new List<Guid>();
        [Field("declined_by")]
        public List<Guid> DeclindeBy { get; set; } = new List<Guid>();
        [Field("absence_description")]
        public List<string> file { get; set; } = new List<string>();  //maybe GUID
    }
}
