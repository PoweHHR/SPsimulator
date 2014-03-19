using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Helpers
{
    static class Helper
    {
        public static bool ValidHexChar(char hex)
        {
            if (hex >= '0' && hex <= '9') return true;
            if (hex >= 'a' && hex <= 'f') return true;
            if (hex >= 'A' && hex <= 'F') return true;
            return false; 
        }
        public static bool ValidHexChar(char[] hex)
        {
            for (int i = 0; i < hex.Length; i++)
            {
                if (!ValidHexChar(hex[i])) return false;
            }
            return true;
        }
        public static bool ValidHexChar(char[] hex,int startfrom,int count)
        {
            for (int i = startfrom; i < startfrom+count; i++)
            {
                if (!ValidHexChar(hex[i])) return false;
            }
            return true;
        }
        public static bool ValidHexChar(string hex, int startfrom, int count)
        {
            for (int i = startfrom; i < startfrom + count; i++)
            {
                if (!ValidHexChar(hex[i])) return false;
            }
            return true;
        }

        public static int SetBit(int data,int bitIndex){
          
            return (data | (1 <<bitIndex));
        }
        public static int ClearBit(int data, int bitIndex)
        {
            return (data &  ~(1 << bitIndex));
        }
        public static ushort Extend12bit(ushort d12)
        {
            if ((d12 & 0x0800) == 0x0800)
            {
                return (ushort)(d12 | 0xF000);
            }
            else
            {
                return (ushort)(d12 & 0x0FFF);
            }
        }
        public static ushort Extend8bit(ushort d8)
        {
            if ((d8 & 0x0080) == 0x0080)
            {
                return (ushort)(d8 | 0xFF00);
            }
            else
            {
                return (ushort)(d8 & 0x00FF);
            }
        }
    }

}
