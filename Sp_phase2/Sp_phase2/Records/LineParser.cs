using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Exceptions;
using SP.Helpers;
namespace SP.Records
{
    class LineParser
    {




        //throws: RecordformatException, IlligalRecordTypeException
        public Record TryParseLine(string recordStr)
        {
            //StringBuilder str = new StringBuilder(recordStr);
            Record r = new Record();
            if (recordStr[0] == '1' || recordStr[0] == '2')
            {
                r.RecordType = byte.Parse(recordStr.Substring(0,1));
                if (recordStr[1] != '-') throw new RecordFormatException("missing dash character from the record at pos.2");
                if (recordStr[4] != ' ') throw new RecordFormatException("missing space character from the record at pos.5");

                if (!Helper.ValidHexChar(recordStr, 2, 2)) throw new RecordFormatException("Byte count in the record contains iilligal characters at pos.3&4");
                r.count = Convert.ToByte(recordStr.Substring(2, 2), 16);

                if (!Helper.ValidHexChar(recordStr, 5, 4)) throw new RecordFormatException("address feild in the record contains iilligal characters at pos.6to9");
                r.address = Convert.ToUInt16(recordStr.Substring(5, 4), 16);

                if (r.RecordType == 1){
                    if (recordStr[9] != ' ') throw new RecordFormatException("missing space character from the record at pos.10");
                    if (!Helper.ValidHexChar(recordStr, 10, 2)) throw new RecordFormatException("checksum in the record contains iilligal characters at pos.11&12");
                    r.checkSum = Convert.ToByte(recordStr.Substring(10, 2), 16);

                }
                else
                {
                    int i = 0;
                    if  (recordStr[9] != '*') throw new RecordFormatException("missing star * character from the record at pos.10");
                    for (i = 10; i < recordStr.Length; i++) { if (recordStr[i] == '*') break; }
                    if (i == recordStr.Length) throw new RecordFormatException("missing the closing star!code of the instruction cannot be determined");
                    if ((i - 11) % 2 != 0) throw new RecordFormatException("code of the instruction contains iilligal characters at pos.11to" + i);
                    if (!Helper.ValidHexChar(recordStr, 10, (i-1) -10   )) throw new RecordFormatException("code of the instruction contains iilligal characters at pos.11to" + i );
                    byte[] bytes = new Byte[(i-11)/2];

                    for (int j = 10, x = 0; j < i; j += 2, x++)
                    {
                        bytes[x] = Convert.ToByte(recordStr.Substring(j, 2), 16);
                    }

                    r.data = bytes;

                    if (!Helper.ValidHexChar(recordStr, i+1, 2)) throw new RecordFormatException("checksum in the record contains iilligal characters at pos."+  (i+2) +"&" + (i+3)   );
                    r.checkSum = Convert.ToByte(recordStr.Substring(i+1, 2), 16);
                }


            }
           
            else
            {
                throw new IlligalRecordTypeException();

            }


            return r;
        }

    }
}
