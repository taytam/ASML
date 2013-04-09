using System;
using System.Configuration;
using System.Windows.Forms;

namespace ASML_N7
{
    public partial class Configuration : Form
    {
        /**Default constructor for the Configuration window**/
        public Configuration()
        {
            InitializeComponent();
            Load_Configuration();
        }

        /**Load the most recent App.Config target file path**/
        private void Load_Configuration()
        {
            /**Load the target path into the Label File_Text**/
            File_Text.Text = ConfigurationManager.AppSettings["Target_File"];
        }

        /**Save the selected file path to the App.Config file**/
        private void Save_Configuration(string filename)
        {
            /**ConfigurationManager object to configure App.Config settings**/
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            /**Find the AppSetting Key "Target_File" and set the value to the filename
             * of the ini/xml file input by the user**/
            config.AppSettings.Settings["Target_File"].Value = filename;

            /**Save the App.Config settings**/
            config.Save(ConfigurationSaveMode.Modified);

            /**Refesh the changes made to the App.Config file**/
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        private void Select_File(object sender, EventArgs e)
        {
            /**Create OpenFileDialog**/ 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            /**Set filter for file extension and default file extension**/
            dlg.DefaultExt = ".xml";
            dlg.DefaultExt = ".ini";
            dlg.Filter = "Xml File (.xml)|*.xml|Ini File (.ini)|*.ini";

            /**Display OpenFileDialog by calling ShowDialog method**/ 
            Nullable<bool> result = dlg.ShowDialog();

            /**Get the selected file name and display in a TextBox**/
            if (result == true)
            {
                /**Open document**/ 
                string filename = dlg.FileName;
                File_Text.Text = filename;
                Save_Configuration(filename);
            }
        }

        private void Open_File(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void Cancel_Button(object sender, EventArgs e)
        {
            Save_Configuration("");
            this.Close();
        }
    }
}
