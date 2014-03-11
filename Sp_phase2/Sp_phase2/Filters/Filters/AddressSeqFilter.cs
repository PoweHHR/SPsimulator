using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;

namespace SP.Filters
{
    class AddressSeqFilter : Filter
    {
        ushort LastAcceptAddress = 0;
        int counter = 0;
        public string Reason;
        public override ErrorLevel IsValidRecord(Record r, int RecordID, int RecordCount)
        {
            if (r.RecordType == 2)
            {
                if (counter == 0)
                {
                    counter++;
                    LastAcceptAddress = r.address;
                }
                else
                {
                    if (r.address > LastAcceptAddress)
                    {
                        LastAcceptAddress = r.address;
                        return ErrorLevel.Valid;
                    }
                    else
                    {

                        Reason = "Address must be incresing: LastAdress: " + string.Format("{0:X}", LastAcceptAddress) + " and this recored adress is : " + string.Format("{0:X}", r.address);
                        return ErrorLevel.Error;
                    }
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
