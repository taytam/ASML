using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASML_N7
{
    internal class IniReader : FileReader
    {
        /**Function that checks different parameters that a group must have inorder to be valid**/

        public bool Validate_Group(string group_text)
        {
            /**Bool is_valid keeps track through out as to whether the group string is valid or not. This bool value will then be returned at the end of the function**/
            bool is_valid;

            /**Bool is assumed to be true at the start of the function**/
            is_valid = true;

            /**Checks to see if the line is empty or NULL**/
            if (string.IsNullOrEmpty(group_text))
            {
                is_valid = false;
                return is_valid;
            }

            /**Check to see if the number of characerts is at least 3 meaning it is at least [*]**/
            if (group_text.Length < 3)
            {
                is_valid = false;
            }

            /**Checks the last character on the line for the closing bracket ']'**/
            if (group_text.Substring(group_text.Length - 1, 1) != "]")
            {
                is_valid = false;
            }

            /**If the previous line is a group file is invalid. The group must have some kind of key following it.**/
            if (Prev_Row_Type == "group")
            {
                is_valid = false;
            }

            /**if the group line is valid set the group_set bool to true to keep track that a group has been declared.**/
            if (is_valid == true)
            {
                Group_Set_Value = true;
            }

            /**Set the prev_row_type to group so when validating for key it can make sure that a key follows a group**/
            Prev_Row_Type = "group";

            /**Return the value of is_valid, true for valid and false for invalid**/
            return is_valid;
        }

        /**Validate_Key checks different parameters that a group must have inorder to be valid**/

        public bool Validate_Key(string key_text)
        {
            bool is_valid;
            is_valid = true;
            string[] key_array = key_text.Split('=');

            if (string.IsNullOrEmpty(key_text))
            {
                is_valid = false;
                return is_valid;
            }
            /**Make sure that the key has a Key value pair. Check if there is an equal siqn**/
            if (key_text.IndexOf("=") == -1)
            {
                is_valid = false;
            }
            /**Check if a group has been declared before a key can be added to the group. Keys
             * cannot come before a target unless at least one target has been declared and 
             * validated. Return false if no target has been declared**/
            if (Group_Set_Value == false)
            {
                is_valid = false;
            }

            /**Checks to see if the format is valid and then whether is an actual value on both sides of the '='**/
            /**Returns true if there is emptry space on the left or right side of the '='**/
            if ((is_valid) &&
                (string.IsNullOrEmpty(key_array[0].ToString()) || string.IsNullOrEmpty(key_array[1].ToString())))
            {
                is_valid = false;
            }
            /**Set the prev_row_type to key**/
            Prev_Row_Type = "key";

            /**Return is_valid, if true key format is valid, if false then key format is invalid**/
            return is_valid;
        }

        /**Read_File function reads in a filename and pulls out the ini content from the file
         * and places it into a list of strings that will then be returned at the end of the
         * function**/

        public override List<string> Read_File(string filename)
        {
            List<string> fileLines = File.ReadAllLines(filename).ToList<string>();
            List<string> outputFile = new List<string>();

            bool isLineValid = true;

            foreach (string line in fileLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    outputFile.Add(string.Empty);
                }
                else if (line.Substring(0, 1) == "[")
                {
                    if (!Validate_Group(line))
                    {
                        isLineValid = false;
                    }
                    else
                    {
                        outputFile.Add(line);
                    }
                }
                else if (line.Substring(0, 1) == ";")
                {
                    outputFile.Add(line);
                }
                else
                {
                    if (!Validate_Key(line))
                    {
                        isLineValid = false;
                    }
                    else
                    {
                        outputFile.Add(line);
                    }
                }
            }

            if (isLineValid == false)
            {
                return null;
            }
            return outputFile;
        }
    }
}
