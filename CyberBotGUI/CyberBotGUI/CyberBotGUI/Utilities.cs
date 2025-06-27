using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Drawing;

namespace CyberBotGUI.Bot
{
    public static class Utilities
    {
        public static void PrintWithColor(SpeechSynthesizer synth, string message, RichTextBox outputBox)
        {
            synth.SpeakAsync(message);

            outputBox.SelectionStart = outputBox.TextLength;
            outputBox.SelectionLength = 0;
            outputBox.SelectionColor = Color.Blue;

            outputBox.AppendText("Bot: " + message + Environment.NewLine);
            outputBox.SelectionColor = outputBox.ForeColor;
            outputBox.ScrollToCaret();
        }

        public static void PrintUserInput(string message, RichTextBox outputBox)
        {
            outputBox.SelectionStart = outputBox.TextLength;
            outputBox.SelectionLength = 0;
            outputBox.SelectionColor = Color.Black;

            outputBox.AppendText("You: " + message + Environment.NewLine);
            outputBox.SelectionColor = outputBox.ForeColor;
            outputBox.ScrollToCaret();
        }
    }
}
