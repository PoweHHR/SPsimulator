using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.MemoryUnit;
using SP.RegistersUnit;


namespace SP.InstructionsUnit
{
    public class Fetch
    {
        Memory memUnit;
        Registers regUnit;
        public Fetch()
        {
        }
        public Fetch(Memory memRefrence,Registers regsRef)
        {
            regUnit = regsRef;
            memUnit = memRefrence;
        }
        public void AssignMemoryandRegRefrence(Memory memRefrence,Registers regsRef)
        {
            regUnit = regsRef;
            memUnit = memRefrence;
        }

        public byte[] FetchNext2bytes()
        {
            return new byte[]{
                memUnit[regUnit[RegistersIndex.PC].value++],
                memUnit[regUnit[RegistersIndex.PC].value++]};
        }
        public ushort FetchNextWord()
        {
            ushort tmp = memUnit.getshortAt(regUnit[RegistersIndex.PC].value) ;
            regUnit[RegistersIndex.PC].value+=2;
            return tmp;
        }

    }
}
