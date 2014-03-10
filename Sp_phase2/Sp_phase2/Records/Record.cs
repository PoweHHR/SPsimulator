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
        
        //Index can be 0 for the lowest byte // 1 for the most significant byte
        public byte Address0(int byteIndex){
            return (byte)(address >> byteIndex*8);

        }
     

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
