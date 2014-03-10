using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Registers
{
    class Register
    {
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
        
        
        #endregion


    }
}
