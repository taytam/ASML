using System;
using System.Collections.Generic;
using System.IO;

namespace N7_GUI_Project1
{
    /**fileWriter is an abstract class used for creating
     * Ini and Xml fileWriter**/
    public abstract class FileWriter
    {
        public abstract void Write_File(List<string> output_file, string filepath);
    }

    /**FilewriterFactory uses the factory design method to 
     * construct different types of file writer. The 
     * FilewriterFactory produces ini and xml file format writers**/
    public class FilewriterFactory
    {
        public FileWriter Check_File_Type(string filename)
        {
            FileWriter file;
            FilewriterFactory factory = new FilewriterFactory();
            string path = Path.GetExtension(filename);


            if (path == ".ini")
            {
                file = factory.CreateFileWriter(fileType.xml);
                return file;
            }
            else if (path == ".xml")
            {
                file = factory.CreateFileWriter(fileType.ini);
                return file;
            }
            else
            {
                Console.WriteLine("invalid file.");
                return null;
            }
        }

        /**Concrete filewriterFactory creates conrete filereader
         * objects either ini writer or xml writer**/
        public FileWriter CreateFileWriter(fileType file_t)
        {
           FileWriter file = null;
            switch (file_t)
            {
                case fileType.ini:
                    file = new Ini_Writer();
                    break;
                case fileType.xml:
                    file = new Xml_Writer();
                    break;
                default:
                    break;
            }
            return file;
        }
    }
}
