using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Records;

namespace SP
{
    class Program
    {
        static void Main(string[] args)
        {

            LineParser l = new LineParser();
            
            //l.TryParseLine("1-00 ffff aa");

            l.TryParseLine("2-ff ffff*ffffff010203*fff");

        }
    }
}
