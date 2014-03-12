using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;
namespace SP.Filters
{
    class ByteCountFilter :Filter
    {

        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
            if (r.RecordType == 1 )
            {
                if (r.count != 3)
                {
                    Reason = "ByteCount is not equal to 3";
                    return ErrorLevel.Error;
                }

            }
            else if (r.RecordType ==2)
            {
                if ((r.data.Length + 3) != r.count)
                {
                    Reason = "ByteCount is not equal to number of record bytes";
                    return ErrorLevel.Error;
                }

                if (r.count > 21)
                {

                    Reason = "ByteCount is larger than expected recored length ";
                    return ErrorLevel.Error;

                }
                if (r.data.Length % 2 != 0)
                {
                    Reason = "instruction or data length is odd";
                    return ErrorLevel.Warning;

                }

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
