using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
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
        private bool setToggle = false;
        Boolean sunday = false;
        Boolean monday = false;
        Boolean tuesday = false;
        Boolean wednesday = false;
        Boolean thursday = false;
        Boolean friday = false;
        Boolean saturday = false;
        SoundModule sound = new SoundModule();

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

            // populate with sounds
            string[] availableSounds = sound.getSounds();
            for (int i = 0; i < availableSounds.Length; i++)
            {
                comboBoxSounds.Items.Add(availableSounds[i]);
            }

        }

        private void UpdateTimeLabel(object sender, String args)
        {

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
            setAlarm.Visibility = Visibility.Visible;
            alarmHandler.currentAlarm.snooze(1);
        }

        private void clickDismiss(object sender, RoutedEventArgs e)
        {
            buttonDismissAlarm.Visibility = Visibility.Collapsed;
            buttonSnoozeAlarm.Visibility = Visibility.Collapsed;
            BackgroundMediaPlayer.Current.Pause();
            setAlarm.Visibility = Visibility.Visible;
            alarmHandler.currentAlarm = null;
        }

        private void confirmClicked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Collapsed;
            setAlarm.IsChecked = false;

            SoundModule newSound = new SoundModule();

            // string which holds 0 or 1 for each day of the week (Sunday = 0th, Monday = 1th, ..., Saturday = 6th)
            string alarmDaysChecked = "";

            //// Build days string before creating alarm
            //if (checkBox_Sunday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Monday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Tuesday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Wednesday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Thursday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Friday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            //if (checkBox_Saturday.IsChecked == true) { alarmDaysChecked += "1"; }
            //else { alarmDaysChecked += "0"; }

            String selectedSound = (String)comboBoxSounds.SelectedItem;
            newSound.setSound(selectedSound);

            DateTime dt = DateTime.Parse(timePicker.Time.ToString());
            alarmHandler.setNewAlarm(dt, alarmDaysChecked, newSound);

            //MediaPlayer mediaPlayer = new MediaPlayer();
            //mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-winsoundevent:Notification.Looping.Alarm10"));
            //mediaPlayer.Play();

                //BackgroundMediaPlayer.Current.SetUriSource(new Uri("ms-winsoundevent:Notification.Looping.Alarm10"));
                //BackgroundMediaPlayer.Current.Pause();
            
        }

        private void setChecked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Visible;
            
        }

        private void setUnchecked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Collapsed;
        }

        private void cancelClicked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Collapsed;
            setAlarm.IsChecked = false;
        }

        private void mondayClicked(object sender, RoutedEventArgs e)
        {
            if (!monday)
            {
                monday = true;
                alarm_days_mon.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                monday = false;
                alarm_days_mon.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void tuesdayClicked(object sender, RoutedEventArgs e)
        {

        }

        private void wednesdayClicked(object sender, RoutedEventArgs e)
        {

        }

        private void thursdayClicked(object sender, RoutedEventArgs e)
        {

        }

        private void fridayClicked(object sender, RoutedEventArgs e)
        {

        }

        private void saturdayClicked(object sender, RoutedEventArgs e)
        {

        }

        private void sundayClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
