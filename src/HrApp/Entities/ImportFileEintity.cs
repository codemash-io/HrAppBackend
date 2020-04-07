using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("import-files")]
    public class ImportFileEintity:Entity
    {
        [Field("creation_date")]
        public DateTime CreationDate { get; set; }
        [Field("is_successfull_imported")]
        public string Successful { get; set; }
        [Field("vacation_balance_file")]
        public List<object> VacationBalanceFile { get; set; }
        

    }
}
