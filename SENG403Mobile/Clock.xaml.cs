using System;
using System.Collections.Generic;
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
        public static event TimeUpdateEvent UpdateTime;
        public Clock()
        {
            this.InitializeComponent();
            dTimer = new DispatcherTimer();
        }
    }
}
