using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Records
{
    abstract class Record
    {
        byte  RecordType; //= 0 or 1
        byte  count;    //because the maximum record count is limited to 79  characters
        ushort address; 
        byte checkSum;
        byte[] data;
    }

    //class Record1 : Record
    //{
    //
    //}
    //class Record2 : Record
    //{
    //    byte[] data;
    //}
}
