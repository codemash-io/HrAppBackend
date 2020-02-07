using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class Trip
    {
        [Field("country")]
        public string Country { get; set; }
        [Field("from")]
        public DateTime BussinessTripStart { get; set; }
        [Field("to")]
        public DateTime BussinessTripEnd { get; set; }
        [Field("notes")]
        public string Notes { get; set; }
        [Field("payment_document")]
        public List<string> PaymentDocuments { get; set; }
        [Field("trip_documents")]
        public List<string> TripDocuments { get; set; }
    }
}
