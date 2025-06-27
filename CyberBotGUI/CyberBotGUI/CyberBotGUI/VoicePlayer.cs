using System;
using System.Media;
using System.Windows.Forms;

namespace CyberBotGUI.Bot
{
    public static class VoicePlayer
    {
        // According to MicrosoftDocs (2025) the SoundPlayer class allows WAV files to be played in .NET applications.
        /*
        MicrosoftDocs (2025)
        The SoundPlayer class enables playback of .wav audio files using methods like Play, PlaySync, and Load.
        It is part of the System.Media namespace and supports playing audio from files, streams, or embedded resources.
        */
        public static void PlayGreeting()
        {
            try
            {
                
                string path = @"C:\Users\zandr\OneDrive\Documents\Schoolwork\greeting.wav - Copy.wav";

                using (SoundPlayer player = new SoundPlayer(path))
                {
                    player.Load();
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing greeting audio: " + ex.Message);
            }
        }
    }
}
