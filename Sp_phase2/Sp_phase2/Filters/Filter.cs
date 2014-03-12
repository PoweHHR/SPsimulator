using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;
namespace SP.Filters
{
    public enum ErrorLevel{
        CannotDetermine,
        Warning,//can be ignored but most shown for the user
        Error, // cannot be ignored and the excution most stop.
        Valid
    }

    public abstract class Filter
    {
        //true means valid
        //false means invalid
        //null means not my job to detect this record// i don't have opnion if it is valid or not
        public abstract ErrorLevel IsValidRecord(Record r,int RecordID,int RecordCount);
        public abstract string GetReason();
        public abstract ErrorLevel GetErrorLevel();

    }
}
