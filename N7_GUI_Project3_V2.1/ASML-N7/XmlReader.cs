using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ASML_N7
{
    internal class xml_reader : FileReader
    {
        /**Read_File function takes in a file name and reads in files
         * with the .xml extenstion each line of the xml file is placed
         * into a List of strings and then the List of strings is returned
         * at the end of the function**/

        public override List<string> Read_File(string filename)
        {
            XmlTextReader reader = new XmlTextReader(filename);
            List<string> output_file = new List<string>();

            StringBuilder content = new StringBuilder();

            reader.WhitespaceHandling = WhitespaceHandling.None;

            bool is_open = false;
            bool targets_set = false;
            output_file.Add("<?xml version='1.0'?>");

            /**Try Catch is used to try and validate whether the xml file that is being read is a valid xml formated file**/
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: /** The node is an element**/
                            /**Validate that the xml file has some list of targets to be read**/
                            if (reader.Name == "Targets")
                            {
                                targets_set = true;
                            }
                            content.Append("<" + reader.Name);

                            while (reader.MoveToNextAttribute()) /**Read the attributes**/
                            {
                                is_open = true;
                                content.Append(" " + reader.Name + "='" + reader.Value + "'");
                            }
                            if (is_open == true)
                            {
                                content.Append("/");
                                is_open = false;
                            }
                            content.Append(">");
                            output_file.Add(content.ToString());
                            content.Clear();
                            break;

                        case XmlNodeType.Text: /**Display the text in each element**/
                            break;

                        case XmlNodeType.EndElement: /**Display the end of the element**/
                            content.Append("</" + reader.Name);
                            content.Append(">");
                            output_file.Add(content.ToString());
                            content.Clear();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("invalid xml. Error: " + ex.Message);
                return null;
            }
            /**If the targets_set returns false then the xml file is not valid and returns null**/
            if ((targets_set != true) || (output_file.Count < 3))
            {
                return null;
            }

            output_file = XmlToIniFormat(output_file);
            /**If the xml file is valid and has targets that could be read the xml was placed into
             * the List<string> output_file and then is returned**/
            return output_file;
        }

        public List<string> XmlToIniFormat(List<string> xmlStrings)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            List<string> output_text = new List<string>();

            if (xmlStrings == null)
            {
                Console.WriteLine("invalid Xml file.");
                return null;
            }

            string new_string = string.Join<string>(String.Empty, xmlStrings);

            using (XmlReader reader = XmlReader.Create(new StringReader(new_string)))
            {
                do
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Target")
                            {
                                output_text.Add("[" + reader.Name + "]");
                                while (reader.MoveToNextAttribute())
                                {
                                    output_text.Add(reader.Name + " = " + reader.Value);
                                }
                                output_text.Add("\n");
                            }
                            break;

                        case XmlNodeType.Text:
                            output_text.Add(reader.Value);
                            break;

                        case XmlNodeType.EndElement:
                            output_text.Add(string.Empty);
                            break;
                    }
                } while (reader.Read());
            }
            return output_text;
        }
    }
}