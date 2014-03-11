using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.MemoryUnit
{
    class Memory
    {
        private byte [] memArr;
        int currentptr;
        byte MostbyteSearch;
        byte LeastByteSearch;
        const int MEMSIZE =64*1024;
        
        public Memory()
        {
            memArr =  new byte[MEMSIZE];
            //initialize all memory contents to one's
            for (int i = 0; i < MEMSIZE; i++)
                memArr[i] = 0xFF;
        }

        //be aware that calling this with ushort 
        public byte this[int address]
        {
            get
            {
                return memArr[address];
            }
            set
            {
                memArr[address] = value;
            }
        }
        //public ushort this[int address]
        //{
        //    get
        //    {
        //        ushort t = memArr[address];
        //        t <<= 8;
        //        t += memArr[address+1]; 
        //        return t;
        //    }
        //    set
        //    {
        //        memArr[address+1] = (byte)value;
        //        memArr[address]   = (byte)(value >> 8);
        //    }
        //}

        public ushort getshortAt(int address)
        {
            ushort t = memArr[address];
            t <<= 8;
            t += memArr[address + 1];
            return t;
        }
        public void setshortAt(int address,ushort value)
        {
            memArr[address + 1] = (byte)value;
            memArr[address] = (byte)(value >> 8);
        }

        
        public void OpenSearchSession(ushort valuetoFind)
        {
            currentptr = 0;
            LeastByteSearch = (byte)valuetoFind;
            MostbyteSearch  = (byte)(valuetoFind >>8);
        }
        public int FindNextByte()
        {
            for (int i = currentptr; i < MEMSIZE; i++)
            {
                if (memArr[i] == LeastByteSearch)
                {
                    currentptr =(i + 1);
                    return i;
                }
            }
            return -1; //not found
        }
        public int FindNextShort()
        {
            for (int i = currentptr; i < MEMSIZE-1; i++)
            {
                if (memArr[i] == MostbyteSearch)
                {
                    if (memArr[i + 1] == LeastByteSearch)
                    {
                        currentptr = (i + 2);//needs logical check , wheather to increment by 1 or 2
                        return i;
                    }
                }
            }
            return -1; //not found
        }



        public string GetGroup16DataString(int address){
            address = address - (address % 16);
            string hex = address.ToString("X4")+":";
            string str =" ";

            for (int i = address; i < 16+address; i++)
            {
                hex += ' ' + memArr[i].ToString("X2");
                if (memArr[i] != 255) str += (char)memArr[i];
                else str += '-';

            }

            return hex + str;
        }

    }
}
