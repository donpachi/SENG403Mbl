using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SENG403Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AlarmHandler alarmHandler;
        public MainPage()
        {
            
            this.InitializeComponent();
            BackgroundMediaPlayer.Current.SetUriSource(new Uri("ms-winsoundevent:Notification.Looping.Alarm10"));
            BackgroundMediaPlayer.Current.IsLoopingEnabled = true;
            BackgroundMediaPlayer.Current.Pause();
            Clock.UpdateTimeEvent += UpdateTimeLabel;
            //Dateblock.Text = DateTime.Now.DayOfWeek.ToString();//DateTime.Now.ToString();
            alarmHandler = new AlarmHandler();
            Alarm.onRing += OnAlarmRing;
            // DateTime.Now.Date.ToString;
        }

        private void UpdateTimeLabel(object sender, String args)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundMediaPlayer.Current.SetUriSource(new Uri("ms-appx:///Sounds/sawarp1.wav"));

                BackgroundMediaPlayer.Current.Play();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    // handle exception.
                    // For example: Log error or notify user problem with file
                }
            }
        }

        private void OnAlarmRing()
        {
            BackgroundMediaPlayer.Current.Play();
            buttonDismissAlarm.Visibility = Visibility.Visible;
            buttonSnoozeAlarm.Visibility = Visibility.Visible;
            setAlarm.Visibility = Visibility.Collapsed;
            timePicker.Visibility = Visibility.Collapsed;
            confirmAlarm.Visibility = Visibility.Collapsed;
        }

        private void clickSnooze(object sender, RoutedEventArgs e)
        {
            buttonDismissAlarm.Visibility = Visibility.Collapsed;
            buttonSnoozeAlarm.Visibility = Visibility.Collapsed;
            BackgroundMediaPlayer.Current.Pause();
            alarmHandler.currentAlarm.snooze(1);

        }

        private void clickDismiss(object sender, RoutedEventArgs e)
        {
            buttonDismissAlarm.Visibility = Visibility.Collapsed;
            buttonSnoozeAlarm.Visibility = Visibility.Collapsed;
            BackgroundMediaPlayer.Current.Pause();
            alarmHandler.currentAlarm = null;
        }

        private void setClicked(object sender, RoutedEventArgs e)
        {
            setAlarm.Visibility = Visibility.Collapsed;
            timePicker.Visibility = Visibility.Visible;
            confirmAlarm.Visibility = Visibility.Visible;
        }

        private void confirmClicked(object sender, RoutedEventArgs e)
        {
            confirmAlarm.Visibility = Visibility.Collapsed;
            timePicker.Visibility = Visibility.Collapsed;
            setAlarm.Visibility = Visibility.Visible;

            DateTime alarmTime = new DateTime();
            alarmTime.Add(timePicker.Time);
            alarmHandler.setNewAlarmTime(timePicker.Time.ToString());
        }
    }
}
