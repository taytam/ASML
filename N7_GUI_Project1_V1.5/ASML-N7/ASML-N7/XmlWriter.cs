using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ASML_N7
{
    class xml_writer : FileWriter
    {
        /**Write_File function accepts a List of ini strings and the original filepath
         * string. Pulls the ini strings out of the list and writes them to XML format
         * using the System.Xml library**/
        public override void Write_File(List<string> output_file, string filepath)
        {
            XElement _root = new XElement("Targets");
            XElement _child = null;

            /**Check to see if the list of strings is null. If its null print invalid file**/
            if (output_file == null)
            {
                Console.WriteLine("invalid file.");
                return;
            }

            /**Loop to pull out all of the strings from the list and place them in
             * Xml format**/
            foreach (string line in output_file)
            {
                if (string.IsNullOrEmpty(line) || line.Substring(0, 1) == ";")
                {
                    continue;
                }
                else if (line.Contains("[Target]"))
                {
                    _child = new XElement("Target");
                    _root.Add(_child);
                }
                else if (line.Contains("="))
                {
                    string[] name_value_pair = line.Split('=');
                    name_value_pair[0] = name_value_pair[0].Replace(" ", "");
                    name_value_pair[1] = name_value_pair[1].Replace(" ", "");
                    XAttribute attr = new XAttribute(name_value_pair[0], name_value_pair[1]);
                    if (_child != null)
                    {
                        _child.Add(attr);
                    }
                }
            }
            /**Save the new Xml file to a file with the same name but uses .xml extension**/
            _root.Save(Path.GetFileNameWithoutExtension(filepath) + ".xml");

        }
    }
}
