using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;
namespace SP.Filters
{
    class CheckSumFilter : Filter
    {
        int counter = 0;
        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
            uint sum = (uint)((uint)r.count + (((uint)r.address) % 256) + ((uint)(r.address) >> 8));

            if (r.RecordType == 2)
            {
                int datalength = r.data.Length;
                for (int i = 0; i < datalength; i++)
                {
                    sum = sum + r.data[i];
                }
            }

            if (sum > 255)
            {
                uint temp = sum;
                sum = temp % 256;
                temp = temp >> 8;
                sum += temp;

            }
            byte ss = (byte)~sum;

            if (ss != r.checkSum)
            {
                Reason = "Check Sum not matched";
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
