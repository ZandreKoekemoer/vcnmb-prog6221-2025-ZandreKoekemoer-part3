using CyberBotGUI.Bot;

namespace CyberBotGUI
{
    public partial class Form1 : Form
    {
        private CyberBot bot;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rtbChat.AppendText(AsciiArt.GetBanner() + Environment.NewLine);
            VoicePlayer.PlayGreeting();

            string name = Microsoft.VisualBasic.Interaction.InputBox("Welcome! Please enter your name:", "CyberBot");
            if (string.IsNullOrWhiteSpace(name))
                name = "User";

            bot = new CyberBot(name);

            AppendToChat($"Bot: Hi {name}! I'm your Cybersecurity Assistant.");
            AppendToChat("Bot: You can ask me about phishing, passwords, privacy, or say 'start quiz', 'add task', or 'show activity log'.");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string userInput = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                MessageBox.Show("Please type a message.");
                return;
            }

            AppendToChat("You: " + userInput);
            txtInput.Clear();

            bot.ProcessInput(userInput, rtbChat);
        }

        private void AppendToChat(string message)
        {
            rtbChat.AppendText(message + Environment.NewLine);
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
