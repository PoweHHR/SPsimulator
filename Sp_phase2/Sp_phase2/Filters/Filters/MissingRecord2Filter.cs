using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;
namespace SP.Filters
{
    class MissingRecord2Filter :Filter
    {
        int counter = 0;
        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {


            if (r.RecordType == 2) counter++;

            if (RecordID == RecordCount - 1 && counter == 0)
            {
                Reason = "no record type 2 in this code , there must be at least one record of type 2 in this code";
                return ErrorLevel.Error;
            }
            return ErrorLevel.Valid;
        }
        public override string GetReason()
        {
            return Reason;
        }
        public override ErrorLevel GetErrorLevel()
        {
            return ErrorLevel.Error;
        }
    }
}
