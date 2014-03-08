using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SP.Readers
{
    class FileReader : Reader
    {
        StreamReader fd=null;
        string fs;
        public FileReader(string FilePath){
            //StreamReader fd = new StreamReader(FilePath);
            fs = FilePath;
        }
        public override void  OpenReadingSession()
        {
            fd = new StreamReader(fs);
        }
        public override  void  OpenReadingSession(string filePath)
        {
            fs = filePath;
            fd = new StreamReader(fs);
        }
        public override string GetNextLine()
        {
            if (fd == null) return null;
            if (fd.EndOfStream)
            {

                fd.Close();
                fd = null;
                return null;
            }
            else
            {
                return fd.ReadLine();
            }
        }


    }
}
