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
        public float AbsenceStart { get; set; }
        
        [Field("end")]
        public float AbsenceEnd { get; set; }
        
        [Field("status")]
        public string Status { get; set; } = AbsenceRequestStatus.Pending.ToString();
        

    }
}
