using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;

namespace SP.Filters
{
    class duplicateRecorcdType1Filter : Filter
    {
        int counter = 0;
        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
          


            if (r.RecordType == 1) counter++;
            if (counter > 1)
            {
                Reason = "More than one record of type one has been found, maximum allowed records of type 1 is one record";
                return ErrorLevel.Error;
            }
            return ErrorLevel.CannotDetermine;
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
