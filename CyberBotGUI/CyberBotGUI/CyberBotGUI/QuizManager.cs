using System.Collections.Generic;

namespace CyberBotGUI.Bot
{
    public class QuizManager
    {
        private int currentQuestionIndex = 0;
        private int score = 0;

        public int Score => score;
        public bool QuizOver => currentQuestionIndex >= questions.Count;

        
        public int TotalQuestions => questions.Count;

       
        public QuizQuestion CurrentQuestion
        {
            get
            {
                if (currentQuestionIndex == 0)
                    return null;
                return questions[currentQuestionIndex - 1];
            }
        }

        private List<QuizQuestion> questions;

        public QuizManager()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion("What should you do if you receive an email asking for your password?", new[] { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" }, 2),
                new QuizQuestion("True or False: Using the same password for all accounts is safe.", new[] { "True", "False" }, 1),
                new QuizQuestion("Which of the following is a strong password?", new[] { "123456", "mypassword", "P@ssw0rd!", "qwerty" }, 2),
                new QuizQuestion("Phishing scams usually try to...", new[] { "Fix your computer", "Steal your personal information", "Help you login faster", "Upgrade your internet" }, 1),
                new QuizQuestion("How often should you update your passwords?", new[] { "Never", "Every few years", "Once a year", "Every few months" }, 3),
                new QuizQuestion("True or False: Public Wi-Fi is always safe to use.", new[] { "True", "False" }, 1),
                new QuizQuestion("What is 2FA short for?", new[] { "Two-Factor Authentication", "Two-Faced Access", "Twice-Forced Access", "None of the above" }, 0),
                new QuizQuestion("Which is the safest way to store passwords?", new[] { "On paper", "In a text file", "In a password manager", "In your email" }, 2),
                new QuizQuestion("Spyware is a type of...", new[] { "Software that protects your system", "Malware that spies on you", "Antivirus", "Firewall" }, 1),
                new QuizQuestion("If a site’s address starts with HTTPS, it means...", new[] { "The site is fake", "It’s encrypted", "It’s hosted in another country", "It’s a scam" }, 1)
            };
        }

        public QuizQuestion GetNextQuestion()
        {
            return questions[currentQuestionIndex++];
        }

        public void CheckAnswer(int selectedOption)
        {
            var question = questions[currentQuestionIndex - 1];
            if (selectedOption == question.CorrectOption)
            {
                score++;
            }
        }

        public class QuizQuestion
        {
            public string Question { get; }
            public string[] Options { get; }
            public int CorrectOption { get; }

            public QuizQuestion(string question, string[] options, int correctOption)
            {
                Question = question;
                Options = options;
                CorrectOption = correctOption;
            }
        }
    }
}
/*
 Stack Overflow. 2021. How to create a Dictionary of List<string> in C#. (Version 2.0) [Source code] https://stackoverflow.com/questions/65588009/how-to-create-a-dictionary-of-liststring [Accessed 23 May 2025]
*/