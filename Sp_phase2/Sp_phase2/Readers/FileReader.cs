using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SP.Readers
{
    public class FileReader : Reader
    {
        /// <summary>
        /// file handler points the file structure in the opertaion system
        /// </summary>
        StreamReader fd=null;
        string fs;
        /// <summary>
        /// Create session of reading file passed on it's path
        /// </summary>
        public FileReader(string FilePath){
            //StreamReader fd = new StreamReader(FilePath);
            fs = FilePath;
        }


        /// <summary>
        /// gives the number os lines in the file
        /// </summary>
        /// <remarks>
        /// this function opens the file when called and scans all newLine marks 
        /// so this is extensive function call it once.
        /// </remarks>
        /// <returns>returns the number of lines in the files</returns>
        public override int GetLineCount()
        {
            int i =0;
            StreamReader r = new StreamReader(fs);
            string s;
            while (!r.EndOfStream) {
                s = r.ReadLine();
                if (s != "" && s.Length > 0) i++;
            }
            r.Close();
            return i;

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
