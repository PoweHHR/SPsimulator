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


    }

}
