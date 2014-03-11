using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;

namespace SP.Filters
{
    class MemoryBoundFilter :Filter
    {

        int counter = 0;
        public string Reason;


        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
            ushort mem = r.address;
            if (mem > 65535)
            {
                Reason = "Err: Address out of Memory bound, Last address is 0xFFFF";
                return ErrorLevel.Error;
            }
            else if (mem >= 64512)
            {
                Reason = "Err: Address on Stack boundry, Last address available is 0xFBFF";
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
