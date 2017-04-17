using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Control> Controls { get; set; }

        List<AlarmContainer> alarmStack = new List<AlarmContainer>();
        AlarmHandler alarmHandler;
        SoundModule sound;

        public MainPage()
        {
            
            this.InitializeComponent();
            //Dateblock.Text = DateTime.Now.DayOfWeek.ToString();//DateTime.Now.ToString();
            alarmHandler = new AlarmHandler();
            ClockUI.RegisterAlarmHandler(alarmHandler);
            Alarm.onRing += OnAlarmRing;
            // DateTime.Now.Date.ToString;

            // populate with sounds
            sound = new SoundModule();
            upcoming_alarm_panel.SetRingtones(sound.getSounds());

            Controls = new ObservableCollection<Control>(alarmStack);
            alarm_listview.ItemsSource = Controls;

        }

        private void OnAlarmRing()
        {
            buttonDismissAlarm.Visibility = Visibility.Visible;
            buttonSnoozeAlarm.Visibility = Visibility.Visible;
            setAlarm.Visibility = Visibility.Collapsed;
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
            string alarmDaysChecked = new_alarm_container.GetDays();

            String selectedSound = new_alarm_container.GetRingtone();
            newSound.setSound(selectedSound);

            DateTime dt = DateTime.Parse(new_alarm_container.GetAlarmTime().ToString());
            Alarm newAlarm = alarmHandler.setNewAlarm(dt, alarmDaysChecked, newSound);
            new_alarm_container.Subscribe(newAlarm);
            PopulateAlarmStackBox(new_alarm_container, newAlarm);
        }

        private void setChecked(object sender, RoutedEventArgs e)
        {
            setalarmcanvas.Visibility = Visibility.Visible;
            new_alarm_container.Reset();
            new_alarm_container.SetAlarmTime(DateTime.Now);
            new_alarm_container.SetRingtones(sound.getSounds());
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

        private void ShowAlarmList(object sender, RoutedEventArgs e)
        {
            main_time_canvas.Visibility = Visibility.Collapsed;
            main_alarm_canvas.Visibility = Visibility.Visible;

        }

        private void ShowTime(object sender, RoutedEventArgs e)
        {
            main_time_canvas.Visibility = Visibility.Visible;
            main_alarm_canvas.Visibility = Visibility.Collapsed;
        }

        private void PopulateAlarmStackBox(AlarmContainer alarmContainer, Alarm alarm)
        {
            AlarmContainer newContainer = new AlarmContainer(alarmContainer);
            newContainer.Subscribe(alarm);
            alarmStack.Add(newContainer);
            Controls.Add(newContainer);
        }
    }
}
