using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Registers
{
    public class Registers
    {

        Register[] regs;
        public Registers()
        {
            regs = new Register[12];
            for (int i = 0; i < regs.Length; i++)
            {
                regs[i] = new Register();
            }
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
