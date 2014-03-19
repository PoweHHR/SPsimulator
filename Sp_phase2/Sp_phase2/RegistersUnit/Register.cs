using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.RegistersUnit
{
    public class Register
    {
#region DEFINE
        public const ushort Z           = 0x0000;
        public const ushort V           = 0x0001;
        public const ushort N           = 0x0002;
        public const ushort C           = 0x0003;
        public const ushort byteSignBit = 0x0007;
        public const ushort shortSignBit= 0x0010;

#endregion
        #region RegisterData
        private int data;
        public byte lowByte{
            get
            {
                return (byte)(data & 0x000000FF);
            }
            set
            {
                data =((data & 0x0000FF00) + value);
            }
        }
        public byte highByte
        {
            get
            {
                return (byte) ( (data>>8) & 0x000000FF) ;
            }
            set
            {

                data = data & 0x000000FF + (value * 256);  
            }
        }
        public ushort value{
            get{
                return (ushort)data;
            }
            set
            {
                data = value;
            }


        }
        #endregion

        #region RegisterManipulations
        public ushort this [ushort bitIndex]
        {
            set{
                if (value !=0)
                    data = data | (1 << bitIndex);
                else
                    data =data & ~(1 << bitIndex);
            }
            get{
                if ( (ushort)(data & (1 << bitIndex))  !=0  ) return 1;
                return 0;
            }

        }



        
        #endregion


    }
}
