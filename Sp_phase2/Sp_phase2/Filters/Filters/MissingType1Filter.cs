﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;

namespace SP.Filters
{
    class MissingType1filter : Filter
    {
        int counter = 0;
        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
            if (r.RecordType == 1) counter++;

            if (RecordID == 0 && counter ==0 && RecordID != RecordCount - 1)
            {
                Reason = "Record type 1 should be at the begining of the code.";
                return ErrorLevel.Warning;
            }

            if (RecordID == RecordCount - 1 && counter == 0)
            {
                Reason = "Missing record type 1; there must be one record type 1 in the code";
                return ErrorLevel.Error;
            }
            else
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
