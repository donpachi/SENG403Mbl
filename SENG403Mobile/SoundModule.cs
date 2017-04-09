﻿using System;
using System.IO;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SENG403Mobile
{
    public class SoundModule
    {
        MediaPlayer player;
        private Boolean playing = false;    //true when sound is looping, false when not.
        string[] availableSounds;           //array to hold the filepath of .wav files in the Sounds folder
        public string currentSound;                //the sound that is currently set to play on this SoundModule

        // No-argument constructor. Populates the availableSounds array
        // with .wav files found in the Sounds folder.
        //Sounds folder should be in the root directory of project (with .xaml and .cs files).
        public SoundModule()
        {
            loadSounds();
            currentSound = "Sounds\\squarearp1.wav";        //default sound
        }

        //set the sound that is to be played by this SoundModule
        public void setSound(string soundPath)
        {
            currentSound = soundPath;
        }

        // Makes the SoundPlayer start looping a sound.
        // Its one parameter is the filepath of the desiried .wav file as a string.
        // *** Usage: use setSound(the sound's filepath) before calling playSound()
        // otherwise can use getSound(index) for the parameter of this method.
        public void playSound()
        {
            try
            {
                player = new MediaPlayer();
                player.Source = MediaSource.CreateFromUri(new Uri(currentSound));
                player.Play();                       //loops the selected sound until stopSound() is called
                playing = true;
                System.Diagnostics.Debug.WriteLine("SHOULD PLAY?");
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("Error: sound file not found");
            }
        }


        // makes the SoundPlayer stop playing a sound.
        public void stopSound()
        {
            player.Pause();
            //player.Stop();
            playing = false;
            player.Dispose();
        }


        // Returns whether or not a sound is playing right now.
        public Boolean isPlaying()
        {
            return playing;
        }


        // Returns all .wav files in the Sounds directory in a string array.
        public String[] getSounds()
        {
            return availableSounds;
        }


        // Returns element i in the availableSounds array.
        public String getSound(int i)
        {
            try
            {
                return availableSounds[i];
            }
            catch (IndexOutOfRangeException)
            {

            }
            return "";
        }


        // populate availableSounds array with the .wav filepaths found in the Sounds folder
        public void loadSounds()
        {
            availableSounds = Directory.GetFiles("Sounds", "*.wav");      //access two directories up to the sounds folder

            // stretch goal: create a popup that informs the user that the sounds folder has no .wav files in it.

            // for debugging
            for (int i = 0; i < availableSounds.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(availableSounds[i]);
            }
        }

    }
}
