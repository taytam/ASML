using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Microsoft.Expression.Encoder.Devices;
using WebcamControl;

namespace ASML_N7
{
    public partial class MainWindow : Window
    {
        /**Creates a MissileLauncher object for the MainWindow class**/

        static MissileLauncher launcher = new MissileLauncher();
        private TargetManager TManager;

        public MainWindow()
        {
            InitializeComponent();

            TManager = TargetManager.GetInstance();

            TManager.AddedTarget += TManager_AddedTarget;


            /**Bind the Video device properties of the
             * Webcam control to the SelectedValue property of 
             * the necessary ComboBox**/
            Binding bndg_1 = new Binding("SelectedValue");
            bndg_1.Source = VidDvcsComboBox;
            WebCamCtrl.SetBinding(Webcam.VideoDeviceProperty, bndg_1);

            /**Create directory for saving video files**/
            string vidPath = @"C:\VideoClips";

            if (Directory.Exists(vidPath) == false)
            {
                Directory.CreateDirectory(vidPath);
            }

            /**Create directory for saving image files**/
            string imgPath = @"C:\WebcamSnapshots";

            if (Directory.Exists(imgPath) == false)
            {
                Directory.CreateDirectory(imgPath);
            }

            /**Set the Webcam video feed properties**/
            WebCamCtrl.VideoDirectory = vidPath;
            WebCamCtrl.VidFormat = VideoFormat.mp4;

            /**Set the Webcam image properties**/
            WebCamCtrl.ImageDirectory = imgPath;
            WebCamCtrl.PictureFormat = ImageFormat.Jpeg;

            WebCamCtrl.FrameRate = 30;
            WebCamCtrl.FrameSize = new System.Drawing.Size(320, 240);

            /**Find a/v devices connected to the machine**/
            FindDevices();

            VidDvcsComboBox.SelectedIndex = 0;
        }

        /**FindDevices looks for all possible webcam devices available on the
         * computer**/
        public void FindDevices()
        {
            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);

            foreach (EncoderDevice dvc in vidDevices)
            {
                VidDvcsComboBox.Items.Add(dvc.Name);
            }
        }

        /**Start_Button_Click start the live video feed**/
        public void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /**Display webcam video on control**/
                WebCamCtrl.StartCapture();

                launcher.command_reset();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
        }

        /**Stop_Button_Click stops the live video feed**/
        public void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            // Stop the display of webcam video.
            WebCamCtrl.StopCapture();
        }

        /**Up Button press to move turret up**/
        public void move_up(object sender, RoutedEventArgs e)
        {
            launcher.command_Up(25);
        }

        /**Down Button press to move turret down**/
        public void move_down(object sender, RoutedEventArgs e)
        {
            launcher.command_Down(25);
        }

        /**Left Button press to move the turret left**/
        public void move_left(object sender, RoutedEventArgs e)
        {
           launcher.command_Left(25);
        }

        /**Right Button press to move the turret right**/
        public void move_right(object sender, RoutedEventArgs e)
        {
            launcher.command_Right(25);
        }

        /**Function fires the turret**/
        public void Fire(object sender, RoutedEventArgs e)
        {
            launcher.command_Fire();
        }

        /**Show_Configuration_Window brings up the file 
         * reader configuration window**/
        public void Show_Configuration_Window(object sender, RoutedEventArgs e)
        {
            
        /**Creates a Configuration window object to that the user can
         * give the program the ini or xml file**/
            Configuration configuration_Window = new Configuration();
            configuration_Window.Show();
        }

        /**Load_Target_Info takes target coordinates that are read from the
         * ini/xml file that is loaded in by the user**/
        public void Load_Target_Info(object sender, RoutedEventArgs e)
        {
            /**Testing Purposes**/
            TargetManager targetManager = TargetManager.GetInstance();
            /*****/

            TargetAdded_Label.Content = "";
            string value = System.Configuration.ConfigurationManager.AppSettings["Target_File"];

            FileReader file;
            FilereaderFactory fileFactory = new FilereaderFactory();
            file = fileFactory.Check_File_Type(value);

            /**If the file has an invalid format handler**/
            if (file == null)
            {
                //Environment.Exit(-1);
                return;
            }

            List<string> outputFile = file.Read_File(value);

            if (outputFile != null)
            {
                Write_Target_Info(outputFile);
            }

            CreateTarget(outputFile);

            CreateLogFile();

        }

        /**Write_Target_Info writes out a list of strings to the ListBox
         * item containing the target information**/
        public void Write_Target_Info(List<string> list)
        {
            targetInformation.Items.Clear();
            foreach (string item in list)
                targetInformation.Items.Add(item); 
        }

        private static int target_counter = 0;

        private void TManager_AddedTarget(object sender, Target target)
        {
            target_counter++;
            TargetAdded_Label.Content = "Targets added " + target_counter;
        }

        public Target CreateTarget(List<string> targetDescription)
        {
            Target target = null;
            bool targetInfo = false;
            int listCount = 0;

            foreach (string line in targetDescription)
            {
                if (line.ToLower() == "[target]")
                {
                    if (targetInfo == true)
                    {
                        BuildTargetList(target);
                        target = null;
                    }
                    targetInfo = true;
                    if (target == null)
                    {
                        target = new Target();
                    }
                }
                if (targetInfo == true)
                {
                    if (line.Contains("xPos"))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double xpos = Convert.ToDouble(nameValuePair[1]);
                        if ((xpos - (int) xpos) >= .5)
                        {
                            target.XPosition = (int) xpos + 1;
                        }
                        else
                        {
                            target.XPosition = (int) xpos;
                        }
                    }
                    if (line.Contains("yPos"))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double ypos = Convert.ToDouble(nameValuePair[1]);
                        if ((ypos - (int) ypos) >= .5)
                        {
                            target.YPosition = (int) ypos + 1;
                        }
                        else
                        {
                            target.YPosition = (int) ypos;
                        }
                    }
                    if (line.Contains("zPos"))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double zpos = Convert.ToDouble(nameValuePair[1]);
                        if ((zpos - (int) zpos) >= .5)
                        {
                            target.ZPosition = (int) zpos + 1;
                        }
                        else
                        {
                            target.ZPosition = (int) zpos;
                        }
                    }
                    if (line.Contains("Name"))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        target.Name = nameValuePair[1];
                    }
                    if (line.Contains("isFriend"))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        if (nameValuePair[1].ToLower() == "true")
                        {
                            target.isFriend = true;
                        }
                        else if (nameValuePair[1].ToLower() == "false")
                        {
                            target.isFriend = false;
                        }
                    }
                }
                listCount++;
                if (listCount == targetDescription.Count)
                {
                    if (targetInfo == true)
                    {
                        BuildTargetList(target);
                        target = null;
                    }
                }
            }
            return target;
        }
        
        public void BuildTargetList(Target target)
        {
            TargetManager targetManager = TargetManager.GetInstance();
            targetManager.Add_Targets(target);
        }

        public void CreateLogFile()
        {
            LogFile file = new LogFile();
            TargetManager targetManager = TargetManager.GetInstance();
            file.TakeinTargetFiles(targetManager.GetList());
        }
    }
}