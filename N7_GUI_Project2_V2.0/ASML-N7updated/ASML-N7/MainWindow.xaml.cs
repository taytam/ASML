using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ASML_N7
{
    public partial class MainWindow : Window
    {
        private int seconds = 0;
        private int second_digit_seconds = 0;
        private int minutes = 0;
        private int second_digit_minutes = 0;
        private int hours = 0;
        private int second_digit_hours = 0;
        private int target_counter = 0;

        private MissileLauncher launcher = new MissileLauncher();
        private TargetManager TManager;
        private KinectSensor sensor;
        private GuiManagerMediator guiManager;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer = null;

        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Intermediate storage for the color data received from the camera
        /// </summary>
        private byte[] colorPixels;

        public MainWindow()
        {
            InitializeComponent();
            TManager = TargetManager.GetInstance();
            TManager.AddedTarget += TManager_AddedTarget;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        public void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
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

        public void DisableButtons()
        {
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            UpButton.IsEnabled = false;
            DownButton.IsEnabled = false;
            LeftButton.IsEnabled = false;
            RightButton.IsEnabled = false;
            FireButton.IsEnabled = false;
        }

        public void EnableButtons()
        {
            UpButton.IsEnabled = true;
            DownButton.IsEnabled = true;
            LeftButton.IsEnabled = true;
            RightButton.IsEnabled = true;
            FireButton.IsEnabled = true;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = true;
        }

        public void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimerReset();
                StartTimer();
                DisableButtons();
                SnDController controller = SnDController.getInstance(comboBoxSelectMode.SelectionBoxItem.ToString());
                Task missileLauncherThread = new Task(controller.SearchAndDestroy);
                missileLauncherThread.Start();

                //updateSubscribe(controller);

                Task timerStop = missileLauncherThread.ContinueWith((antecedent) =>
                {
                    EnableButtons();
                    TimerStop();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
        }

        //public void updateSubscribe(SnDController controller)
        //{
        //    labelPhiValue.Content = controller.guiManger.Phi;
        //    labelThetaValue.Content = controller.guiManger.Theta;
        //}

        public void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            EnableButtons();
            TimerReset();
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }
        }

        public void StartTimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void TimerStop()
        {
            dispatcherTimer.Stop();
        }

        public void TimerReset()
        {
            seconds = 0;
            second_digit_seconds = 0;
            minutes = 0;
            second_digit_minutes = 0;
            hours = 0;
            second_digit_hours = 0;

            labelTimerValue.Content = second_digit_hours + "" + hours + ":" + second_digit_minutes + "" + minutes + ":" + second_digit_seconds + "" + seconds;
        }

        public void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (seconds > 8)
            {
                if ((seconds >= 9) && (second_digit_seconds == 5))
                {
                    seconds = 0;
                    second_digit_seconds = 0;
                    minutes = minutes + 1;
                }
                else
                {
                    second_digit_seconds = second_digit_seconds + 1;
                    seconds = 0;
                }
            }
            else
            {
                seconds = seconds + 1;
            }

            labelTimerValue.Content = second_digit_hours + "" + hours + ":" + second_digit_minutes + "" + minutes + ":" + second_digit_seconds + "" + seconds;

            if (minutes == 3)
            {
                labelTimerValue.Content = second_digit_hours + "" + hours + ":" + second_digit_minutes + "" + minutes + ":" + second_digit_seconds + "" + seconds;
                dispatcherTimer.Stop();
            }
        }

        public void move_up(object sender, RoutedEventArgs e)
        {
            launcher.command_Up(20);
        }

        public void move_down(object sender, RoutedEventArgs e)
        {
            launcher.command_Down(20);
        }

        public void move_left(object sender, RoutedEventArgs e)
        {
            launcher.command_Left(20);
        }

        public void move_right(object sender, RoutedEventArgs e)
        {
            launcher.command_Right(20);
        }

        public void Fire(object sender, RoutedEventArgs e)
        {
            launcher.command_Fire();
        }

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

        public void TManager_AddedTarget(object sender, Target target)
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
                        target.coordinateConversion(target.XPosition, target.YPosition, target.ZPosition);
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
                    if ((line.Contains("xPos")) || (line.Contains("x =")))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double xpos = Convert.ToDouble(nameValuePair[1]);
                        if ((xpos - (int)xpos) >= .5)
                        {
                            target.XPosition = (int)xpos + 1;
                        }
                        else
                        {
                            target.XPosition = (int)xpos;
                        }
                    }
                    if ((line.Contains("yPos")) || (line.Contains("y =")))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double ypos = Convert.ToDouble(nameValuePair[1]);
                        if ((ypos - (int)ypos) >= .5)
                        {
                            target.YPosition = (int)ypos + 1;
                        }
                        else
                        {
                            target.YPosition = (int)ypos;
                        }
                    }
                    if ((line.Contains("zPos")) || (line.Contains("z =")))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        double zpos = Convert.ToDouble(nameValuePair[1]);
                        if ((zpos - (int)zpos) >= .5)
                        {
                            target.ZPosition = (int)zpos + 1;
                        }
                        else
                        {
                            target.ZPosition = (int)zpos;
                        }
                    }
                    if ((line.Contains("Name")) || (line.Contains("name")))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        target.Name = nameValuePair[1];
                    }
                    if ((line.Contains("isFriend")) || (line.Contains("friend")))
                    {
                        string[] nameValuePair = line.Split('=');
                        nameValuePair[0] = nameValuePair[0].Replace(" ", "");
                        nameValuePair[1] = nameValuePair[1].Replace(" ", "");
                        if ((nameValuePair[1].ToLower() == "true") || (nameValuePair[1].ToLower() == "yes"))
                        {
                            target.isFriend = true;
                        }
                        else if ((nameValuePair[1].ToLower() == "false") || (nameValuePair[1].ToLower() == "no"))
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

        public void Start_Video_Click(object sender, RoutedEventArgs e)
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

        public void Stop_Video_Click(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
                this.Image.Source = null;
            }
        }
    }
}