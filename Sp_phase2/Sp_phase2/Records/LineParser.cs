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
        /// <summary>
        /// takes line string represents the record , checks for any syntax errors , then extract the infromation and export it as Record Object
        /// </summary>
        /// <remarks>
        /// if the function failed to parse the record it could throw one of the following exceptions: 
        /// 1-MissingRecordParameters
        /// 2-RecordFormatException
        /// 3-IlligalRecordTypeException
        /// 4-EmptyRecordLineException
        /// </remarks>
        /// <param name="recordStr">string line represents the record</param>
        /// <returns>return Record object contains the infromation about the record parsed from the line</returns>
        public Record TryParseLine(string recordStr)
        {
            //StringBuilder str = new StringBuilder(recordStr);
            Record r = new Record();

            if (recordStr.Length ==0 || recordStr.CompareTo("")==0 ) throw new EmptyRecordLineException();


            if (recordStr[0] == '1' || recordStr[0] == '2')
            {
                r.RecordType = byte.Parse(recordStr.Substring(0,1));
                if (recordStr.Length < 4) throw new MissingRecordParameters("the record contains missing parameters; byte count is missing");
                if (recordStr[1] != '-') throw new RecordFormatException("missing dash character from the record at pos.2");

                if (!Helper.ValidHexChar(recordStr, 2, 2)) throw new RecordFormatException("Byte count in the record contains iilligal characters at pos.3&4");
                r.count = Convert.ToByte(recordStr.Substring(2, 2), 16);

                if (recordStr.Length < 5) throw new RecordFormatException("missing space character from the record at pos.5");
                if (recordStr[4] != ' ') throw new RecordFormatException("missing space character from the record at pos.5");

               
                if (recordStr.Length < 9) throw new MissingRecordParameters("The record contains missing parameters; the address is missing");
                if (!Helper.ValidHexChar(recordStr, 5, 4)) throw new RecordFormatException("address feild in the record contains iilligal characters at pos.6to9");
                r.address = Convert.ToUInt16(recordStr.Substring(5, 4), 16);

                if (r.RecordType == 1){
                    if (recordStr.Length < 12) throw new MissingRecordParameters("the record contains missing parameters;checksum is missing");
                    if (recordStr[9] != ' ') throw new RecordFormatException("missing space character from the record at pos.10");
                    if (!Helper.ValidHexChar(recordStr, 10, 2)) throw new RecordFormatException("checksum in the record contains iilligal characters at pos.11&12");
                    r.checkSum = Convert.ToByte(recordStr.Substring(10, 2), 16);

                    if (recordStr.Length > 12) throw new RecordFormatException("the record contains extra unknown bytes");

                }
                else
                {
                    int i = 0;
                    if (recordStr.Length < 10) throw new MissingRecordParameters("the record contains missing parameters;instruction data is missing");  
                    if  (recordStr[9] != '*') throw new RecordFormatException("missing star * character from the record at pos.10");
                    for (i = 10; i < recordStr.Length; i++) { if (recordStr[i] == '*') break; }
                    if (i == recordStr.Length ) throw new RecordFormatException("missing the closing star!code of the instruction cannot be determined");
                             
                    if ((i - 10) % 2 != 0 ) throw new RecordFormatException("code of the instruction contains odd number of characters at pos.11to" + i +"; failed to parse as bytes");
                    if (i - 9 == 1) throw new RecordFormatException("the record contains missing parameters;instruction data is missing");
                    if (!Helper.ValidHexChar(recordStr, 10, (i) -10   )) throw new RecordFormatException("code of the instruction contains iilligal characters at pos.11to" + i );
                    byte[] bytes = new Byte[(i-10)/2];

                    for (int j = 10, x = 0; j < i; j += 2, x++)
                    {
                        bytes[x] = Convert.ToByte(recordStr.Substring(j, 2), 16);
                    }

                    r.data = bytes;
                    if (recordStr.Length < i + 3) throw new MissingRecordParameters("the record contains missing parameters;checksum is missing");
                    if (!Helper.ValidHexChar(recordStr, i+1, 2)) throw new RecordFormatException("checksum in the record contains iilligal characters at pos."+  (i+2) +"&" + (i+3)   );
                    r.checkSum = Convert.ToByte(recordStr.Substring(i+1, 2), 16);

                    if (recordStr.Length > i+3) throw new RecordFormatException("the record contains extra unknown bytes");


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
