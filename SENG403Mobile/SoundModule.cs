
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SENG403Mobile
{

    public class SoundModule
    {

        //MediaPlayer mediaPlayer = new MediaPlayer();
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


        public async Task playSound()
        {/*
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Sounds");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(currentSound);
            mediaPlayer.AutoPlay = false;
            mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);

            mediaPlayer.MediaOpened += soundOpened;

            mediaPlayer.IsLoopingEnabled = true;
            */
        }


        private void soundOpened(MediaPlayer sender, object args)
        {
            sender.Play();
        }


        //set the sound that is to be played by this SoundModule
        public void setSound(string soundPath)
        {
            currentSound = soundPath;
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
