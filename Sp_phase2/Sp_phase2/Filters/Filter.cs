using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;
namespace SP.Filters
{
    abstract class Filter
    {
        //true means valid
        //false means invalid
        //null means not my job to detect this record// i don't have opnion if it is valid or not
        public abstract Nullable<bool> IsValidRecord(Record r);
        public abstract string GetReason();

    }
}
