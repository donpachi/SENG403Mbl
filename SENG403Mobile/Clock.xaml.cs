using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SENG403Mobile
{
    #region namespace constants 
    //NOTE** should really have this in a constants class elsewhere
    public enum RenderMode { RenderSecond, RenderMinutes, RenderHour, RenderAll, DontRender }

    public class CONSTANTS
    {
        public const int TICK_INTERVAL_MS = 125;
        public const int MS_IN_SEC = 1000;
        public const double DEG_PER_SEC = 6;
        public const double DEG_PER_HOUR = 30;
        public const double DEG_PER_MIN = 6;
        public const double SEC_IN_MIN = 60;
        public const double MIN_IN_HR = 60;
    }
    #endregion

    public sealed partial class Clock : UserControl
    {
        private static Mutex mutex = new Mutex();
        private static double hourOffset = 0;
        public double HourOffset { get { return hourOffset; } set { hourOffset = value; } }
        private static double minuteOffset = 0;
        public double MinuteOffset { get { return minuteOffset; } set { minuteOffset = value; } }
        public double degreeInterval;
        DispatcherTimer dTimer;
        private double secondDegrees, minuteDegrees, hourDegrees, requestedMinuteAngle, requestedHourAngle;
        private double currHour, currMin, currSec;
        private string date, timestring, meridiem;
        Boolean animateClock;
        DateTime currentDateTime;

        public delegate void TimeUpdateEvent(object o, String arg);
        public static event TimeUpdateEvent UpdateTimeEvent;

        public Clock()
        {
            this.InitializeComponent();
            degreeInterval = (double)(CONSTANTS.DEG_PER_SEC * CONSTANTS.TICK_INTERVAL_MS) / CONSTANTS.MS_IN_SEC;
            dTimer = new DispatcherTimer();
            dTimer.Tick += DTimer_Tick;
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, CONSTANTS.TICK_INTERVAL_MS);
            animateClock = true;
            UpdateTime();
            UpdateTimeLabel();
            SynchronizeHands();
            dTimer.Start();

            //timezone?
        }

        #region time-specific functions
        private void UpdateTime()
        {
            String[] cultureNames = { "en-US" };
            currentDateTime = DateTime.Now.AddHours(hourOffset);
            currentDateTime = currentDateTime.AddMinutes(minuteOffset);
            var culture = new CultureInfo(cultureNames[0]);
            string currentTime = currentDateTime.ToString(culture);
            string[] dateTimeElements = currentTime.Split(' ');
            date = dateTimeElements[0];
            timestring = dateTimeElements[1];
            meridiem = dateTimeElements[2];
            string[] timeElements = dateTimeElements[1].Split(':');
            currHour = Convert.ToDouble(timeElements[0]);               //add UTC offset from time zone
            currMin = Convert.ToDouble(timeElements[1]);
            currSec = Convert.ToDouble(timeElements[2]);
        }

        private void SynchronizeHands()
        {
            ComputeAngles();
            RenderAngles(RenderMode.RenderAll);
        }

        private void ComputeAngles()
        {
            secondDegrees = currSec * CONSTANTS.DEG_PER_SEC;
            minuteDegrees = (currMin * CONSTANTS.DEG_PER_SEC) + (currSec / CONSTANTS.SEC_IN_MIN) * 6;
            hourDegrees = (currHour * CONSTANTS.DEG_PER_HOUR) + (currMin / CONSTANTS.MIN_IN_HR) * 30;
        }

        private void RenderAngles(RenderMode renderMode)
        {
            if (renderMode == RenderMode.RenderSecond)
            {
                RotateTransform transform = new RotateTransform();
                transform.Angle = secondDegrees;
                transform.CenterX = second_hand_image.Width / 2;
                transform.CenterY = second_hand_image.Height;
                secondDegrees = (secondDegrees + degreeInterval) >= 360 ? 0 : secondDegrees + degreeInterval;
                second_hand_image.RenderTransform = transform;
            }

            else if (renderMode == RenderMode.RenderMinutes)
            {
                RotateTransform transform = new RotateTransform();
                transform.Angle = minuteDegrees;
                transform.CenterX = minute_hand_image.Width / 2;
                transform.CenterY = minute_hand_image.Height;
                minute_hand_image.RenderTransform = transform;
            }
            else if (renderMode == RenderMode.RenderHour)
            {
                RotateTransform transform = new RotateTransform();
                transform.Angle = hourDegrees;
                transform.CenterX = hour_hand_image.Width / 2;
                transform.CenterY = hour_hand_image.Height;
                hour_hand_image.RenderTransform = transform;
            }
            else if (renderMode == RenderMode.RenderAll)
            {
                RotateTransform transform = new RotateTransform();
                transform.Angle = secondDegrees;
                transform.CenterX = second_hand_image.Width / 2;
                transform.CenterY = second_hand_image.Height;
                secondDegrees = (secondDegrees + degreeInterval) >= 360 ? 0 : secondDegrees + degreeInterval;
                second_hand_image.RenderTransform = transform;

                transform = new RotateTransform();
                transform.Angle = minuteDegrees;
                transform.CenterX = minute_hand_image.Width / 2;
                transform.CenterY = minute_hand_image.Height;
                minute_hand_image.RenderTransform = transform;

                transform = new RotateTransform();
                transform.Angle = hourDegrees;
                transform.CenterX = hour_hand_image.Width / 2;
                transform.CenterY = hour_hand_image.Height;
                hour_hand_image.RenderTransform = transform;
            }
            else throw new NotImplementedException("Unexpected analog clock render mode");
        }

        private void DTimer_Tick(object sender, object e)
        {
            if (animateClock)
            {
                RenderAngles(RenderMode.RenderSecond);
                if (secondDegrees % CONSTANTS.DEG_PER_SEC == 0)
                {    //every second update
                    UpdateTime();
                    ComputeAngles();
                    RenderAngles(RenderMode.RenderMinutes);
                    UpdateTimeLabel();
                    FireUpdateTime(GetDate());
                }
                if (minuteDegrees % CONSTANTS.DEG_PER_HOUR == 0)
                {   //every minute update
                    RenderAngles(RenderMode.RenderHour);
                }
            }
        }

        private double CalculateAngle(Point dst, Point src)
        {
            double angle = Math.Atan((dst.Y - src.Y) / (dst.X - src.X));
            angle = angle * 180 / Math.PI;
            if (dst.X < src.X)
            {
                angle += 180;
            }
            return angle += 90; //translate to clock-oriented degrees (0 at the top)
        }

        private void UpdateTimeLabel()
        {
            string sec, min, hour;
            if (currSec < 10)
                sec = "0" + currSec.ToString();
            else
                sec = currSec.ToString();
            if (currMin < 10)
                min = "0" + currMin.ToString();
            else
                min = currMin.ToString();
            if (currHour < 10)
                hour = "  " + currHour.ToString();
            else
                hour = currHour.ToString();
            if (meridiem == "PM" && (currHour + 12 < 24))
                hour = (currHour + 12).ToString();
            else if (meridiem == "AM" && currHour + 12 == 24)
                hour = "00";
            if (currHour == 12 && currMin == 0 && currSec >= 0)
                FireUpdateTime(GetDate());  //update Datelabel
            sec_text.Text = sec;
            min_text.Text = min;
            hour_text.Text = hour;
        }

        private void UpdateTimeLabel(double m, double h)
        {
            string sec, min, hour;
            if (currSec < 10)
                sec = "0" + currSec.ToString();
            else
                sec = currSec.ToString();
            if (m < 10)
                min = "0" + m.ToString();
            else
                min = m.ToString();
            if (h < 10)
                hour = "  " + h.ToString();
            else
                hour = h.ToString();
            if (meridiem == "PM")
                hour = (h + 12).ToString();
            sec_text.Text = sec;
            min_text.Text = min;
            hour_text.Text = hour;
        }
        #endregion

        #region class-specific functions
        public static String GetDate()
        {
            DateTime now = Now();
            DateTime time = new DateTime(now.Year, now.Month, now.Day);
            return time.ToString("D");
        }

        public void DisableAnimations()
        {
            animateClock = false;
        }

        public void EnableAnimations()
        {
            animateClock = true;
        }

        public static DateTime Now()
        {
            return DateTime.Now.AddHours(hourOffset).AddMinutes(minuteOffset);
        }
        #endregion

        #region event handlers
        private void FireUpdateTime(String arg)
        {
            UpdateTimeEvent(this, arg);
        }

        private void OnTimeZoneChange(double offset)
        {
            hourOffset = offset;
            minuteOffset = 0;
            UpdateTime();
            UpdateTimeLabel();
            SynchronizeHands();
        }
        #endregion
    }
}
