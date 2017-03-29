﻿using SENG403Mobile;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.Media.Playback;

namespace SENG403Mobile
{
    /// <summary>
    /// Alarmhandler that handles setting up, creating and ending alarms.
    /// It also handles starting a system clock and determining the current day of the week.
    /// </summary>

    public class AlarmHandler
    {
        static public event EventHandler CustomEvent;

        static public void RaiseMyCustomEvent(object sender, EventArgs args)
        {
            if (CustomEvent != null) CustomEvent(sender, args);
        }

        // Create a list of Alarms
        public List<Alarm> alarmList;

        // The current alarm; once an alarm has been triggered it is the current alarm
        // Until the user dismisses it or snoozes it
        public Alarm currentAlarm;

        // Constructor for AlarmHandler class
        public AlarmHandler()
        {
            // The Currently RINGING alarm, if applicable
            this.alarmList = new List<Alarm>();

            // Start the clock
            startclock();
        }


        /// <summary>
        /// Returns the current list of alarms in the alarmList arraylist.
        /// </summary>
        public Alarm[] getAlarms()
        {
            Alarm[] theAlarms = new Alarm[alarmList.Count];
            alarmList.CopyTo(theAlarms);
            return theAlarms;
        }

        /// <summary>
        /// Returns the currently ringing/active alarm
        /// </summary>
        /// <returns>The currently ringing alarm.</returns>
        public Alarm getCurrentAlarm()
        {
            return currentAlarm;
        }

        /// <summary>
        /// End the alarm and set it to the next scheduled date if repeat is true
        /// Otherwise, delete the alarm from the alarm list
        /// </summary>
        /// <param name="alarm">The alarm object to end or set to repeat</param>
        public void endAlarm(Alarm alarm)
        {
            // If the repeat value is set to TRUE for the alarm, reset it
            // So that it repeats at the same time the next week
            if (alarm.getRepeatVal() == true)
            {
                alarm.reset();
            }

            // Otherwise, delete the alarm from the alarm list
            else { deleteAlarm(alarm); }
        }

        /// <summary>
        /// Remove the specified alarm from alarmList even if it is set to repeat
        /// </summary>
        /// <param name="alarm">Alarm object to delete from the alarm list.</param>
        public void deleteAlarm(Alarm alarm)
        {
            alarmList.Remove(alarm);
        }

        /// <summary>
        /// Start the timer, create a tick event and set tick interval to one second
        /// </summary>
        private void startclock()
        {
            // Create a timer object
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);   // Set the timer interval to one second
            //timer.Tick += tickevent;    // After every one second, we trigger a tick event
            timer.Tick += Timer_Tick;
            timer.Start();  // Start the timer
        }

        private void Timer_Tick(object sender, object e)
        {
            // Get the CURRENT date and time in the form: 'yyyy-mm-dd hh:mm:ss AM/PM' (i.e: 2017-01-28 12:20:00 PM)
            TimeSpan time = DateTime.Now.TimeOfDay;//.ToString();//Clock.Now().TimeOfDay;
            String currTime = DateTime.Now.ToString("HH:mm:ss");

            // Creates the day object
            //int day = dayofweek(dateAndTime.Day, dateAndTime.Month, dateAndTime.Year);

            // Get only the time from the DateTime object
            //String time = dateAndTime.ToString();

            // Check every second if the current time is the one we're checking for
            // If so, set the alarm to ring
            foreach (Alarm alarm in alarmList)
            {
                // If the current time is one of the alarms, then check if the day is also correct
                //if (time.Equals(alarm.getTime()))
                if (currTime == alarm.getTimeOnly())
                {

                    // Play the alarm and set the current alarm to this alarm
                    currentAlarm = alarm;
                    currentAlarm.setRinging(true);
                    //BackgroundMediaPlayer.Current.Play();
                }
            }
        }

        /// <summary>
        /// Handle the input data once the "set alarm" button is pressed
        /// Days string should be 7 digits long, "1" represents a selected day, "0" represents a non-selected day
        /// </summary>
        /// <param name="time">The time the alarm is set to trigger on</param>
        /// <param name="days">The days the alarm is set to trigger on</param>
        /// <param name="alarmSound">The alarm sound set to play once the alarm goes off.</param>
        public void setNewAlarm(DateTime time)
        {
            // Create a new alarm and append it to the alarmList
            alarmList.Add(new Alarm(time));
        }

        public void setNewAlarmTime(String time)
        {
            alarmList.Add(new Alarm(time));
        }
    }



    /// <summary>
    /// Alarm object containing all relevant information as to when the alarm goes off and what alarm sound it plays.
    /// </summary>
    public class Alarm
    {
        String timeString;
        TimeSpan timespan;
        DateTime settime;
        DateTime time;
        String days;
        Boolean repeat = false;
        bool currentlyRinging;
        String message;
        //SoundModule alarmSound;

        public delegate void AlarmEvent();
        public static event AlarmEvent onRing;

        /// <summary>
        /// Return whether the alarm is ringing
        /// </summary>
        public void AlarmRinging()
        {
            //if (onRing != null)
            onRing();
        }

        // Alarm constructor
        public Alarm(DateTime time)
        {
            this.time = time;
            this.settime = time;
            this.days = days;
            if (message != "")
            {
                this.message = message;
            }
            else
            {
                message = "Alarm Triggered";
            }
            if (days != "0000000") { repeat = true; }
        }

        public Alarm(String time)
        {
            this.timeString = time;
            this.timespan = TimeSpan.Parse(time);

        }

        public String getMessage() { return message; }

        public void setMessage(String msg)
        {
            message = msg;
        }

        /// <summary>
        /// Return the time this alarm is set to ring.
        /// </summary>
        /// <returns>String representation of the time the alarm is set to ring.</returns>
        public String getDateTime()
        {
            return time.ToString();
        }

        /// <summary>
        /// Return the days the alarm is set to ring on. 
        /// </summary>
        /// <returns>Days the alarm is ringing on.</returns>
        public String getDays() { return days; }

        /// <summary>
        /// Snoozes (delays) the alarm by the amount of minutes input
        /// </summary>
        /// <param name="minutes">Minutes that the user wants to snooze the alarm for.</param>
        public void snooze(Double minutes)
        {
            time = Clock.Now().AddMinutes(minutes);
            String currTime = DateTime.Now.ToString("HH:mm:ss");
            timespan = TimeSpan.Parse(currTime).Add(new TimeSpan(0, (int)minutes, 0));
            //timespan = DateTime.Now.TimeOfDay.Add(new TimeSpan(00, (int)minutes, 00));
            timeString = timespan.ToString();
            BackgroundMediaPlayer.Current.Pause();
        }

        /// <summary>
        /// Reset the alarm to the original pre-snooze configuration.
        /// </summary>
        public void reset() { time = settime; }

        /// <summary>
        /// Return whether the alarm is set to repeat or not
        /// </summary>
        /// <returns>The repeat value of the alarm.</returns>
        public Boolean getRepeatVal() { return repeat; }

        /// <summary>
        /// Set the boolean value true/false depending on whether the alarm is ringing
        /// </summary>
        /// <param name="val">Whether the alarm is ringing or not (true/false).</param>
        public void setRinging(bool val)
        {
            this.currentlyRinging = val;
            if (val == true)
            {
                //alarmSound.playSound();
                AlarmRinging();
                BackgroundMediaPlayer.Current.Play();
            }
            else {
                BackgroundMediaPlayer.Current.Pause();
               // alarmSound.stopSound();
            }
        }

        /// <summary>
        /// Set the repeat value based on the boolean input.
        /// </summary>
        /// <param name="val"></param>
        public void setRepeat(bool val)
        {
            this.repeat = val;
        }

        /// <summary>
        /// Set the setTime value.
        /// </summary>
        /// <param name="dt"></param>
        public void setSetTime(DateTime dt)
        {
            this.settime = dt;
        }

        /// <summary>
        /// Return whether the alarm is currently ringing or not.
        /// </summary>
        /// <returns>Boolean value representing the alarms ringing state.</returns>
        public bool isRinging()
        {
            return this.currentlyRinging;
        }

        /// <summary>
        /// Return the set time.
        /// </summary>
        /// <returns></returns>
        public String getSetTime() { return this.settime.ToString(); }

        /// <summary>
        /// Return the time.
        /// </summary>
        /// <returns></returns>
        public String getTime() { return this.time.ToString(); }

        /// <summary>
        /// Return the repeat value.
        /// </summary>
        /// <returns></returns>
        public String getRepeat() { return this.repeat.ToString(); }

        public String getTimeOnly()
        {
            return this.timeString;
        }

        /// <summary>
        /// Return whether the alarm is currently ringing.
        /// </summary>
        /// <returns></returns>
        public String getCurrentlyRinging() { return this.currentlyRinging.ToString(); }

        /// <summary>
        /// Return the alarm sound of the alarm.
        /// </summary>
        /// <returns></returns>
        //public String getSound() {
        //    return this.alarmSound.currentSound;
        //}

        public int getHour() { return this.settime.Hour; }
        public int getMinute() { return this.settime.Minute; }
    }
}
