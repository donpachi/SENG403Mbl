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
        AlarmConstruct alarmConstruct;

        public AlarmContainer()
        {
            this.InitializeComponent();
            alarmConstruct = new AlarmConstruct();
        }

        #region Checked Event Handlers
        private void SundayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void SaturdayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void FridayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void ThursdayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void WednesdayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void TuesdayChecked(object sender, RoutedEventArgs e)
        {

        }

        private void MondayChecked(object sender, RoutedEventArgs e)
        {

        }
#endregion

        #region Unchecked Event Handlers
        private void SundayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SaturdayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void FridayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ThursdayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void WednesdayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void TuesdayUnchecked(object sender, RoutedEventArgs e)
        {

        }

        private void MondayUnchecked(object sender, RoutedEventArgs e)
        {

        }
        #endregion


        private class AlarmConstruct
        {
            private bool m, t, w, r, f, sa, sn;
            public AlarmConstruct()
            {

            }
        }
    }
}
