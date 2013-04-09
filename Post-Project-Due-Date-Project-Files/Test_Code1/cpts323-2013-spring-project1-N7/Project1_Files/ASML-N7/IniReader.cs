using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASML_N7
{
    class IniReader : FileReader
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
            if ((is_valid) && (string.IsNullOrEmpty(key_array[0].ToString()) || string.IsNullOrEmpty(key_array[1].ToString())))
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
        public override List<string> Read_File(string filename, List<Target> targets)
        {
            List<string> file_lines = File.ReadAllLines(filename).ToList<string>();
            List<string> output_file = new List<string>();

            Target target = new Target();

            TargetManager targetManager = TargetManager.GetInstance();

            bool is_line_valid = true;

            bool isTargetInfo = false;

            foreach (string line in file_lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    output_file.Add(string.Empty);
                }
                else if (line.Substring(0, 1) == "[")
                {
                    if (!Validate_Group(line))
                    {
                        is_line_valid = false;
                    }
                    else
                    {
                        output_file.Add(line);
                    }
                    if (isTargetInfo == true)
                    {
                        targetManager.Add_Targets(target);
                        isTargetInfo = false;
                    }

                }
                else if (line.Substring(0, 1) == ";")
                {
                    output_file.Add(line);
                }
                else
                {
                    if (!Validate_Key(line))
                    {
                        is_line_valid = false;
                    }
                    else
                    {
                        isTargetInfo = true;
                        output_file.Add(line);
                        string[] name_value_pair = line.Split('=');
                        name_value_pair[0] = name_value_pair[0].Replace(" ", "");
                        name_value_pair[1] = name_value_pair[1].Replace(" ", "");
                        if(name_value_pair[0] == "xPos")
                        {
                            double xpos = Convert.ToDouble(name_value_pair[1]);
                            if ((xpos - (int)xpos) >= .5)
                            {
                                target.XPosition = (int)xpos + 1;
                            }
                            else
                            {
                                target.XPosition = (int)xpos;
                            }
                        }
                        if (name_value_pair[0] == "yPos")
                        {
                            double ypos = Convert.ToDouble(name_value_pair[1]);
                            if ((ypos - (int)ypos) >= .5)
                            {
                                target.YPosition = (int)ypos + 1;
                            }
                            else
                            {
                                target.YPosition = (int)ypos;
                            }
                        }
                        if (name_value_pair[0] == "zPos")
                        {
                            double zpos = Convert.ToDouble(name_value_pair[1]);
                            if ((zpos - (int)zpos) >= .5)
                            {
                                target.ZPosition = (int)zpos + 1;
                            }
                            else
                            {
                                target.ZPosition = (int)zpos;
                            }
                        }
                        if (name_value_pair[0] == "Name")
                        {
                            target.Name = name_value_pair[1];
                        }
                        if (name_value_pair[0] == "isFriend")
                        {
                            if (name_value_pair[1].ToLower() == "true")
                            {
                                target.isFriend = true;
                            }
                            else if (name_value_pair[1].ToLower() == "false")
                            {
                                target.isFriend = false;
                            }
                        }
                    }
                }
            }

            if (is_line_valid == false)
            {
                return null;
            }

            return output_file;
        }
    }
}
