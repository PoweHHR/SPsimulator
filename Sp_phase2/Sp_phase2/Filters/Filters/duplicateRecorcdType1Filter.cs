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
            if (RecordID == 0 && r.RecordType !=1 && RecordID != RecordCount -1)
            {
                Reason = "Record type 1 should be at the begining of the code.";
                return ErrorLevel.Warning;
            }

           
            if (r.RecordType ==1 )counter++;
            if (counter > 1)
            {
                Reason = "More than one record of type one has been found, maximum allowed records of type 1 is one record";
                return ErrorLevel.Error;
            }
            if (RecordID == RecordCount - 1 && counter ==0)
            {
                Reason = "no record type 1 in this code , there must be one record of type 1 in this code";
            }
            if (counter < 2) return ErrorLevel.Valid;
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
