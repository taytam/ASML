using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Kinect;

namespace ASML_N7
{

        /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>

        static MissileLauncher launcher = new MissileLauncher();
        private TargetManager TManager;
        private KinectSensor sensor;
        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Intermediate storage for the color data received from the camera
        /// </summary>
        private byte[] colorPixels;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.

        /**Creates a MissileLauncher object for the MainWindow class**/

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        /**Start_Button_Click start the live video feed**/
        public void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the color stream to receive color frames
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }
        }

        /**Stop_Button_Click stops the live video feed**/
        public void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            // Start the sensor!
            try
            {
                this.sensor.Stop();
            }
            catch (IOException)
            {
                this.sensor = null;
            }
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
                CreateTarget(outputFile);
                CreateLogFile();
            }



        }


        /**Write_Target_Info writes out a list of strings to the ListBox
         * item containing the target information**/
        public void Write_Target_Info(List<string> list)
        {
            targetInformation.Items.Clear();
            target_counter = 0;
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

        private void StartSearchAndDestroy(object sender, RoutedEventArgs e)
        {
            
        }
    }
}