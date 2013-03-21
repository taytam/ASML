using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ASML_N7
{
    class IniWriter : FileWriter
    {
        /**Write_File function takes in a list of xml strings, that will be written
         * to an ini formatted file, and the original filename. After the content
         * has been pulled from the list and the file has been written the content
         * will be written to a new file with the same name but use the .ini extension**/
        public override void Write_File(List<string> output_file, string filename)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            List<string> output_text = new List<string>();

            if (output_file == null)
            {
                Console.WriteLine("invalid file.");
                return;
            }

            string new_string = string.Join<string>(String.Empty, output_file);

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
            Write(output_text, filename);
        }

        public void Write(List<string> list, string filename)
        {

            using (StreamWriter sw = File.CreateText(Path.GetFileNameWithoutExtension(filename) + ".ini"))
            {
                foreach (string line in list)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}
