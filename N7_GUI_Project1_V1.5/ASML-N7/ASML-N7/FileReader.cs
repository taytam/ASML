using System;
using System.Collections.Generic;
using System.IO;

namespace ASML_N7
{
    /**fileReader is an abstract class used for creating
     * Ini and Xml filereaders**/
    public abstract class FileReader
    {
        /**Bool _isInvalidFile keeps track if whether
         * the file format is valid or not**/
        private bool _isInvalidFile = false;

        /**Bool _groupSet keeps track to make sure
         * that a key does not appear unless followed
         * by a group**/
        private bool _groupSet = false;

        /**String _prevRowType keeps track as to whether 
         * the previous line was either a group or key**/
        private string _prevRowType = null;

        /**Read_File holds all of the lines in the file that
         * is read in and holds them as a list of strings**/
        public abstract List<string> Read_File(string filename, List<Target> targets);

        /**Setter/Getter for _isInvalidFile bool**/
        public bool Invalid_File
        {
            get { return _isInvalidFile; }
            set { _isInvalidFile = value; }
        }

        /**Setter/Getter for _groupSet bool**/
        public bool Group_Set_Value
        {
            get { return _groupSet; }
            set { _groupSet = value; }
        }

        /**Setter/Getter for _prevRowType bool**/
        public string Prev_Row_Type
        {
            get { return _prevRowType; }
            set { _prevRowType = value; }
        }
    }

    /**FilereaderFactory uses the factory design method to 
     * construct different types of file readers. The 
     * FilereaderFactory produces ini and xml file format readers**/
    public class FilereaderFactory
    {
        /**Check_File_Type is used for checking the file type
         * and then returning the right type of FileReader**/
        public FileReader Check_File_Type(string filename)
        {
            
            /**FileReader will be used as an inherited class to
             * create the FileReader**/
            FileReader file;

            /**Create a FilereaderFactory object to create the file
             * reader**/
            FilereaderFactory factory = new FilereaderFactory();

            /**String that will get the file extension and place the
             * file extension in a string called path**/
            string path = Path.GetExtension(filename);

            switch (path)
            {
                case ".ini":
                    file = factory.CreateFileReader(fileType.ini);
                    return file;
                case ".xml":
                    file = factory.CreateFileReader(fileType.xml);
                    return file;
                default:
                    Console.WriteLine("invalid file.");
                    return null;
            }
        }

        /**CreateFileReader returns a FileReader of type ini_reader/xml_reader**/
        public FileReader CreateFileReader(fileType file_t) 
        { 
            FileReader file = null;
            switch (file_t)
            {
                case fileType.ini:
                    file = new IniReader();
                    break;
                case fileType.xml:
                    file = new xml_reader();
                    break;
                default:
                    break;
            }
            return file;
        }
    }

    /**Enumeration for the fileType of Reader or Writer ini or xml**/
    public enum fileType
    {
            ini,
            xml
    }
}