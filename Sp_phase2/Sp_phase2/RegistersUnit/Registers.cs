using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Registers
{
    class Registers
    {

        Register[] regs;
        public Registers()
        {
            regs = new Register[10];
        }

        public Register this[RegistersIndex idx]
        {
            get
            {
                return regs[(int)idx];
            }
        }

    }
}
