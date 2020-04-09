using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("rivile-imports")]
    public class ImportFileEintity:Entity
    {
        [Field("creation_date")]
        public DateTime CreationDate { get; set; }
        [Field("is_successful_imported")]
        public bool Successful { get; set; }
        [Field("vacation_balance_file")]
        public List<object> VacationBalanceFile { get; set; }


    }
}
