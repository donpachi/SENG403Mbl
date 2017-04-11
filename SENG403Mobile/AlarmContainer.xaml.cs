using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AlarmContainer : UserControl
    {
        private enum DaysIndex { Sunday = 0, Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6 }

        int hour, min;
        Alarm alarm;
        List<ToggleButton> buttons = new List<ToggleButton>(7);
        char[] daysChecked = { '0', '0', '0', '0', '0', '0', '0' };
        bool inSettings;

        public AlarmContainer()
        {
            this.InitializeComponent();
            InitButtonArray();
            inSettings = false;
        }

        public AlarmContainer(DateTime dt)
        {
            TimeSpan ts = dt.TimeOfDay;
            hour = ts.Hours;
            min = ts.Minutes;
            alarm_hour_text.Text = ts.Hours.ToString("D2");
            alarm_min_text.Text = ts.Minutes.ToString("D2");
        }

        #region Initialization Code
        private void InitButtonArray()
        {
            buttons.Add(alarm_days_sun);
            buttons.Add(alarm_days_mon);
            buttons.Add(alarm_days_tue);
            buttons.Add(alarm_days_wed);
            buttons.Add(alarm_days_thu);
            buttons.Add(alarm_days_fri);
            buttons.Add(alarm_days_sat);
        }

        public void Subscribe(Alarm alarm)
        {
            this.alarm = alarm;
            SetAlarmTime_UI();
            SetAlarmDays_UI();
        }
        #endregion

        public void SetSounds(string[] args)
        {
            foreach (string s in args)
            {
                sound_combobox.Items.Add(s);
            }
        }

        public string GetDays()
        {
            return new string(daysChecked);
        }

        public string GetRingtone()
        {
            return (string)sound_combobox.SelectedItem;
        }

        public TimeSpan GetAlarmTime()
        {
            return new TimeSpan(hour, min, 0);
        }

        public void SetAlarmTime(DateTime dt)
        {
            hour = dt.TimeOfDay.Hours;
            min = dt.TimeOfDay.Minutes;
            alarm_hour_text.Text = hour.ToString("D2");
            alarm_min_text.Text = min.ToString("D2");
        }

        #region Checked Event Handlers
        private void SundayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Sunday] = '1';
            UpdateAlarmDays();
        }

        private void SaturdayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Saturday] = '1';
            UpdateAlarmDays();
        }

        private void FridayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Friday] = '1';
            UpdateAlarmDays();
        }

        private void ThursdayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Thursday] = '1';
            UpdateAlarmDays();
        }

        private void WednesdayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Wednesday] = '1';
            UpdateAlarmDays();
        }

        private void TuesdayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Tuesday] = '1';
            UpdateAlarmDays();
        }

        private void MondayChecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Monday] = '1';
            UpdateAlarmDays();
        }
        #endregion

        #region Unchecked Event Handlers
        private void SundayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Sunday] = '0';
            UpdateAlarmDays();
        }

        private void SaturdayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Saturday] = '0';
            UpdateAlarmDays();
        }

        private void FridayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Friday] = '0';
            UpdateAlarmDays();
        }

        private void ThursdayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Thursday] = '0';
            UpdateAlarmDays();
        }

        private void WednesdayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Wednesday] = '0';
            UpdateAlarmDays();
        }

        private void TuesdayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Tuesday] = '0';
            UpdateAlarmDays();
        }

        private void MondayUnchecked(object sender, RoutedEventArgs e)
        {
            daysChecked[(int)DaysIndex.Monday] = '0';
            UpdateAlarmDays();
        }
        #endregion

        #region UI Updaters
        private void SetAlarmTime_UI()
        {
            hour = alarm.getHour();
            min = alarm.getMinute();
            alarm_hour_text.Text = hour.ToString("D2");
            alarm_min_text.Text = min.ToString("D2");
        }

        private void SetAlarmDays_UI()
        {
            char[] days = alarm.getDays().ToCharArray();
            for (int index = (int)DaysIndex.Sunday; index <= (int)DaysIndex.Saturday; index++)
            {
                if (days[index] == '1')
                {
                    buttons[index].IsChecked = true;
                }
                else if (days[index] == '0')
                {
                    buttons[index].IsChecked = false;
                }
            }
        }
        #endregion


        private void UpdateAlarmDays()
        {
            try
            {
                string alarmDays = new string(daysChecked);
                alarm.setDays(alarmDays);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void EditAlarmTimeTap(object sender, TappedRoutedEventArgs e)
        {
            TimePickerFlyout timeFlyout = new TimePickerFlyout()
            {
                Placement = FlyoutPlacementMode.Full
            };
            timeFlyout.Time = new TimeSpan(hour, min, 0);
            timeFlyout.TimePicked += TimeFlyout_TimePicked;
            timeFlyout.ShowAt(this);
        }

        private void TimeFlyout_TimePicked(TimePickerFlyout sender, TimePickedEventArgs args)
        {
            TimeSpan pickedTime = args.NewTime;
            hour = pickedTime.Hours;
            min = pickedTime.Minutes;
            alarm_hour_text.Text = pickedTime.Hours.ToString("D2");
            alarm_min_text.Text = pickedTime.Minutes.ToString("D2");
            //alarm.setDateTime(pickedTime.Hours, pickedTime.Minutes);
        }

        private void AlarmSettingsTap(object sender, TappedRoutedEventArgs e)
        {
            
            if (inSettings)
            {
                inSettings = false;
                alarm_construct_canvas.Visibility = Visibility.Visible;
                alarm_edit_canvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                inSettings = true;
                alarm_construct_canvas.Visibility = Visibility.Collapsed;
                alarm_edit_canvas.Visibility = Visibility.Visible;
            }
        }
    }
}
