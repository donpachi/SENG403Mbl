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

        public MainPage()
        {
            
            this.InitializeComponent();
            Clock.UpdateTimeEvent += UpdateTimeLabel;
            //Dateblock.Text = DateTime.Now.DayOfWeek.ToString();//DateTime.Now.ToString();
            alarmHandler = new AlarmHandler();
            Alarm.onRing += OnAlarmRing;
            // DateTime.Now.Date.ToString;

            // populate with sounds
            SoundModule sound = new SoundModule();
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
            buttonDismissAlarm.Visibility = Visibility.Visible;
            buttonSnoozeAlarm.Visibility = Visibility.Visible;
            setAlarm.Visibility = Visibility.Collapsed;
            timePicker.Visibility = Visibility.Collapsed;
            confirmAlarm.Visibility = Visibility.Collapsed;
        }

        private void clickSnooze(object sender, RoutedEventArgs e)
        {
            alarmHandler.getCurrentAlarm().snooze(1);
            alarmHandler.currentAlarm = null;
            buttonDismissAlarm.Visibility = Visibility.Collapsed;
            buttonSnoozeAlarm.Visibility = Visibility.Collapsed;
            setAlarm.Visibility = Visibility.Visible;
        }

        private void clickDismiss(object sender, RoutedEventArgs e)
        {
            Alarm ringingAlarm = alarmHandler.getCurrentAlarm();
            ringingAlarm.setRinging(false);
            alarmHandler.endAlarm(ringingAlarm);
            alarmHandler.currentAlarm = null;

            buttonDismissAlarm.Visibility = Visibility.Collapsed;
            buttonSnoozeAlarm.Visibility = Visibility.Collapsed;
            setAlarm.Visibility = Visibility.Visible;
        }

        private void confirmClicked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Collapsed;
            setAlarm.IsChecked = false;

            SoundModule newSound = new SoundModule();

            Alarm[] alarmList = alarmHandler.getAlarms(); // This holds all the alarms that have been set

            // Use alarm.setDateTime(int hour, int minutes) to set a new time for the alarm
            // Use alarm.setDays(String days) to set new days for the alarm

            // string which holds 0 or 1 for each day of the week (Sunday = 0th, Monday = 1th, ..., Saturday = 6th)
            // TODO: Need to set the appropriate days as '1', everything else should work after that
            string alarmDaysChecked = "0000000";

            //// Build days string before creating alarm - need to update for button implementation instead

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
    }
}
