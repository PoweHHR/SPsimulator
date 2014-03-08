using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Records
{
    class Record
    {
        public byte  RecordType; //= 0 or 1
        public byte  count;    //because the maximum record count is limited to 79  characters
        public ushort address; 
        public byte checkSum;
        public byte[] data;
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
