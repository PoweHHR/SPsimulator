using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.RegistersUnit
{
    public class Registers
    {

        Register[] regs;
        public Registers()
        {
            regs = new Register[11];
            for (int i = 0; i < regs.Length; i++)
            {
                regs[i] = new Register();
            }
        }
        public void ResetRegisters()
        {
            for (int i = 0; i < regs.Length; i++)
            {
                regs[i].value = 0;
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
