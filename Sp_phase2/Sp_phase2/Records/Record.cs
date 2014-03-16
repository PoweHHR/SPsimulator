using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Records
{
    public class Record
    {
        /// <summary>
        /// refers to the record type; 1 or 2
        /// </summary>
        public byte  RecordType; //= 0 or 1
        /// <summary>
        /// holds the bytes count of the record
        /// </summary>
        public byte  count;    //because the maximum record count is limited to 79  characters
        /// <summary>
        /// holds the address of the instruction data
        /// </summary>
        public ushort address;
        /// <summary>
        /// holds the checksum of the record
        /// </summary>
        public byte checkSum;
        /// <summary>
        /// holds the instruction data included in the record
        /// </summary>
        public byte[] data;
        
        //Index can be 0 for the lowest byte // 1 for the most significant byte
        /// <summary>
        /// gives the lower or upper byte of the address
        /// </summary>
        /// <param name="byteIndex">zero refers to the lower byte and one to the upper byte</param>
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
