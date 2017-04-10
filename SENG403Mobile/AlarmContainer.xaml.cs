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

        Alarm alarm;
        List<ToggleButton> buttons = new List<ToggleButton>(7);
        char[] daysChecked = { '0', '0', '0', '0', '0', '0', '0' };

        public AlarmContainer()
        {
            this.InitializeComponent();
            InitButtonArray();
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
            alarm_hour_text.Text = alarm.getHour().ToString();
            alarm_min_text.Text = alarm.getMinute().ToString();
        }

        private void SetAlarmDays_UI()
        {
            string[] days = alarm.getDays().Split();
            foreach (int index in Enum.GetValues(typeof(DaysIndex)))
            {
                if (days[index] == "1")
                {
                    buttons[index].IsChecked = true;
                }
            }
        }
        #endregion

        private void UpdateAlarmDays()
        {
            string alarmDays = new string(daysChecked);
        }

    }
}
