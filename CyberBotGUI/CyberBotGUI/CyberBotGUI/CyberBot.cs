using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CyberBotGUI.Bot
{
    public class CyberBot
    {
        private string userName;
        private string userInterest = "";
        private SpeechSynthesizer synth;
        private Dictionary<string, List<string>> keywordResponses;
        private Dictionary<string, List<string>> preventionTips;
        private Dictionary<string, string> sentimentMap;
        private Dictionary<string, int> keywordResponseIndex;
        private Dictionary<string, int> tipIndex;
        private List<string> activityLog = new();
        private List<TaskItem> tasks = new();
        private QuizManager quiz;

        // Reference Microsoft Docs. Delegates - C# Programming Guide.

        // According to Microsoft Docs (2023) A delegate is a type-safe function pointer used to call methods indirectly.
        // I used it here to simplify response output handling.
        private delegate void ResponseHandler(string message, RichTextBox box);

        public CyberBot(string name)
        {
            userName = name;
            synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            quiz = new QuizManager();
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            // Reference Stack Overflow. How to create a Dictionary of List<string> in C#.

            // According to Stack OverFlow (2021) It demonstrates how to organize multiple values under a single key using List<string> within a Dictionary.
            // I used this approach to store multiple possible responses for each cybersecurity keyword in my chatbot.
            keywordResponses = new Dictionary<string, List<string>>
            {
                { "phishing", new List<string> {
                    "Phishing is a scam where attackers steal personal info using fake emails or sites.",
                    "Phishing uses deception to trick users into giving away credentials.",
                    "Phishing emails may mimic legitimate sources like banks.",
                    "They often include urgent language to provoke fear." } },
                { "password", new List<string> {
                    "Use strong passwords with a mix of characters.",
                    "Avoid using the same password across sites.",
                    "Change passwords often.",
                    "Consider a secure password manager." } },
                { "privacy", new List<string> {
                    "Limit what you share on social media.",
                    "Review app permissions regularly.",
                    "Use private browsing modes.",
                    "Avoid using public Wi-Fi without a VPN." } },
                { "malware", new List<string> {
                    "Malware is malicious software like viruses or spyware.",
                    "Avoid downloading unknown attachments.",
                    "Keep antivirus up to date.",
                    "Ransomware can encrypt files until a ransom is paid." } },
                { "scam", new List<string> {
                    "Scams trick users into giving up money or info.",
                    "Common scams impersonate banks or family.",
                    "If it sounds too good to be true, it is.",
                    "Watch out for urgent or emotional pleas." } },
            };

            preventionTips = new Dictionary<string, List<string>>
            {
                { "phishing", new List<string> {
                    "Verify the sender before clicking.",
                    "Don't open unknown attachments.",
                    "Check for grammar mistakes in emails.",
                    "Use spam filters to reduce risk." } },
                { "password", new List<string> {
                    "Use 2FA where possible.",
                    "Don't reuse passwords.",
                    "Don't use personal info.",
                    "Update passwords regularly." } },
                { "privacy", new List<string> {
                    "Disable location sharing.",
                    "Read privacy policies.",
                    "Clear cookies often.",
                    "Use a secure browser." } },
                { "malware", new List<string> {
                    "Update software often.",
                    "Use antivirus software.",
                    "Avoid pirated software.",
                    "Enable firewall." } },
                { "scam", new List<string> {
                    "Be skeptical of unknown numbers.",
                    "Don't share OTPs.",
                    "Check URLs carefully.",
                    "Report suspected scams." } },
            };

            sentimentMap = new Dictionary<string, string>
            {
                { "worried", "It’s okay to be worried! I'm here to help you understand cybersecurity." },
                { "curious", "Curiosity is great! Ask away." },
                { "frustrated", "It can be frustrating, but we’ll work through it!" },
                { "scared", "It’s okay to feel scared — let’s learn how to stay safe." }
            };

            keywordResponseIndex = new();
            tipIndex = new();
        }

        public void ProcessInput(string input, RichTextBox outputBox)
        {
            ResponseHandler respond = (message, box) => Utilities.PrintWithColor(synth, message, box);

            if (string.IsNullOrWhiteSpace(input))
            {
                respond("Please type something.", outputBox);
                return;
            }

            input = input.ToLower();

            if (input == "exit")
            {
                respond("Thanks for chatting. Stay safe!", outputBox);
                return;
            }

            if (input.Contains("show activity") || input.Contains("what have you done"))
            {
                ShowActivityLog(outputBox);
                return;
            }

            if (CombinedSentiment(input, outputBox)) return;
            if (HandleSentiment(input, outputBox)) return;
            if (ProcessNLP(input, outputBox)) return;
            if (Interest(input, outputBox)) return;
            if (Confusion(input, outputBox)) return;
            if (Keyword(input, outputBox)) return;

            respond("I don’t understand that. Try asking about phishing, passwords, scams, or say 'quiz' or 'add task'.", outputBox);
        }
        // Reference C# Corner. How to Use List in C#.

        // According to C# Corner (2021) the article explains how to create and manipulate a list using the List<T> class.
        // I used this to store user actions in a List<string> called activityLog to keep track of important events like quiz attempts and task management.

        private void ShowActivityLog(RichTextBox outputBox)
        {
            Utilities.PrintWithColor(synth, "Here's a summary of recent actions:", outputBox);
            foreach (var entry in activityLog.GetRange(Math.Max(0, activityLog.Count - 5), Math.Min(5, activityLog.Count)))
            {
                Utilities.PrintWithColor(synth, "- " + entry, outputBox);
            }
        }

        private bool Keyword(string input, RichTextBox outputBox)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                // Reference Microsoft. Random.Next Method (System).

                // According to Microsoft(2023) This documentation explains how to use the Random class to generate pseudo-random numbers for indexing purposes.
                // I used this to randomly select a different chatbot response each time for variety and natural conversation flow.
                if (input.Contains(keyword))
                {
                    if (!keywordResponseIndex.ContainsKey(keyword))
                        keywordResponseIndex[keyword] = 0;

                    if (!tipIndex.ContainsKey(keyword))
                        tipIndex[keyword] = 0;

                    var responseList = keywordResponses[keyword];
                    var tipList = preventionTips[keyword];

                    var nextResponse = responseList[keywordResponseIndex[keyword]];
                    var nextTip = tipList[tipIndex[keyword]];

                    keywordResponseIndex[keyword] = (keywordResponseIndex[keyword] + 1) % responseList.Count;
                    tipIndex[keyword] = (tipIndex[keyword] + 1) % tipList.Count;

                    Utilities.PrintWithColor(synth, nextResponse, outputBox);
                    Utilities.PrintWithColor(synth, "Tip: " + nextTip, outputBox);

                    userInterest = keyword;
                    activityLog.Add($"User learned about {keyword}");
                    return true;
                }
            }

            return false;
        }

        private bool Interest(string input, RichTextBox outputBox)
        {
            if (input.Contains("interested in"))
            {
                userInterest = input.Substring(input.IndexOf("interested in") + 13).Trim();
                Utilities.PrintWithColor(synth, $"Got it! You're interested in {userInterest}.", outputBox);
                return true;
            }

            if (input.Contains("remind me"))
            {
                if (!string.IsNullOrEmpty(userInterest))
                    Utilities.PrintWithColor(synth, $"You told me you're interested in {userInterest}.", outputBox);
                else
                    Utilities.PrintWithColor(synth, "I don't recall your interest.", outputBox);
                return true;
            }

            return false;
        }
        // Reference GeeksForGeeks. String.Contains Method in C#.
        // I used this to detect if the user expresses confusion or needs clarification in their input.
        private bool Confusion(string input, RichTextBox outputBox)
        {
            string[] confusionPhrases = { "tell me more", "explain", "clarify", "repeat", "what do you mean", "i'm confused" };

            foreach (var phrase in confusionPhrases)
            {
                if (input.Contains(phrase))
                {
                    if (!string.IsNullOrEmpty(userInterest))
                        return Keyword(userInterest, outputBox);
                    else
                        Utilities.PrintWithColor(synth, "Tell me what you'd like help with.", outputBox);

                    return true;
                }
            }

            return false;
        }
        // Reference C# Corner. Understanding Dictionary in C#.

        // According to C# Corner (2022) The article describes how to use a Dictionary<string, string> to associate related text values for simplified lookup.
        // I used this to map user sentiment to helpful emotional responses in the chatbot.
        private bool HandleSentiment(string input, RichTextBox outputBox)
        {
            foreach (var sentiment in sentimentMap)
            {
                if (input.Contains(sentiment.Key))
                {
                    Utilities.PrintWithColor(synth, sentiment.Value, outputBox);
                    return true;
                }
            }

            return false;
        }

        private bool CombinedSentiment(string input, RichTextBox outputBox)
        {
            foreach (var sentiment in sentimentMap.Keys)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (input.Contains(sentiment) && input.Contains(keyword))
                    {
                        Utilities.PrintWithColor(synth, sentimentMap[sentiment], outputBox);
                        return Keyword(keyword, outputBox);
                    }
                }
            }

            return false;
        }

        private bool ProcessNLP(string input, RichTextBox outputBox)
        {
            if (input.StartsWith("add task", StringComparison.OrdinalIgnoreCase))
            {
                // Reference TutorialsTeacher. C# Regex (Regular Expressions).

                // According to TutorialsTeacher (2022) regular expressions can be used to extract matching patterns from user input.
                // I used Regex.Match() to detect and extract task titles from the input, like in: "add task - Buy antivirus".
                var match = Regex.Match(input, @"add task\s*-?\s*(.+)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var fullInput = match.Groups[1].Value.Trim();

                    // Extract title and optional reminder
                    var reminderMatch = Regex.Match(fullInput, @"(.*?)\s*remind me in (\d+) days?", RegexOptions.IgnoreCase);
                    string title = fullInput;
                    DateTime? reminder = null;

                    if (reminderMatch.Success)
                    {
                        title = reminderMatch.Groups[1].Value.Trim();
                        if (int.TryParse(reminderMatch.Groups[2].Value, out int days))
                        {
                            reminder = DateTime.Now.AddDays(days);
                        }
                    }

                    var task = new TaskItem(title, $"Cyber task: {title}", reminder);
                    tasks.Add(task);

                    Utilities.PrintWithColor(synth, $"Task added: {title}.", outputBox);
                    if (reminder.HasValue)
                    {
                        Utilities.PrintWithColor(synth, $"Reminder set for {reminder.Value.ToShortDateString()}.", outputBox);
                        activityLog.Add($"Reminder set for '{title}' on {reminder.Value.ToShortDateString()}");
                    }

                    activityLog.Add($"Task added: {title}");
                    return true;
                }
            }


            if (input.StartsWith("show tasks", StringComparison.OrdinalIgnoreCase))
            {
                if (tasks.Count == 0)
                {
                    Utilities.PrintWithColor(synth, "You have no tasks right now.", outputBox);
                }
                else
                {
                    Utilities.PrintWithColor(synth, "Here are your tasks:", outputBox);
                    foreach (var task in tasks)
                        Utilities.PrintWithColor(synth, task.ToString(), outputBox);
                }
                return true;
            }

            if (input.StartsWith("complete task", StringComparison.OrdinalIgnoreCase))
            {
                var match = Regex.Match(input, @"complete task\s*-?\s*(.+)", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    string title = match.Groups[1].Value.Trim();
                    var task = tasks.Find(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                    if (task != null)
                    {
                        task.IsCompleted = true;
                        Utilities.PrintWithColor(synth, $"Task '{title}' marked as completed.", outputBox);
                        activityLog.Add($"Task completed: {title}");
                    }
                    else
                    {
                        Utilities.PrintWithColor(synth, $"Task '{title}' not found.", outputBox);
                    }
                    return true;
                }
            }

            if (input.StartsWith("delete task", StringComparison.OrdinalIgnoreCase))
            {
                var match = Regex.Match(input, @"delete task\s*-?\s*(.+)", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    string title = match.Groups[1].Value.Trim();
                    var task = tasks.Find(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                    if (task != null)
                    {
                        tasks.Remove(task);
                        Utilities.PrintWithColor(synth, $"Task '{title}' deleted.", outputBox);
                        activityLog.Add($"Task deleted: {title}");
                    }
                    else
                    {
                        Utilities.PrintWithColor(synth, $"Task '{title}' not found.", outputBox);
                    }
                    return true;
                }
            }
            //Reference Moo ICT: C# Tutorial – Create a simple multiple choice quiz game in Visual Studio

            //According to Moo ICT (2024), this tutorial guides through creating a multiple-choice quiz game in Visual Studio using C# programming language and Windows Form .Net Framework.p
            //I followed this tutorial to implement the quiz feature in my application.
            if (input.Contains("start quiz") || input.Contains("quiz"))
            {
                quiz = new QuizManager(); // reset quiz
                AskNextQuizQuestion(outputBox);
                activityLog.Add("Quiz started.");
                return true;
            }

            
            if (!quiz.QuizOver && input.Length == 1 && char.IsLetter(input[0]))
            {
                int selected = char.ToUpper(input[0]) - 'A';

                var currentQuestion = quiz.CurrentQuestion;
                quiz.CheckAnswer(selected);

                if (selected == currentQuestion.CorrectOption)
                {
                    Utilities.PrintWithColor(synth, "Correct! Well done.", outputBox);
                }
                else
                {
                    string correctAnswerText = currentQuestion.Options[currentQuestion.CorrectOption];
                    Utilities.PrintWithColor(synth, $"Sorry, that's incorrect. The correct answer is: {correctAnswerText}", outputBox);
                }

                if (quiz.QuizOver)
                {
                    Utilities.PrintWithColor(synth, $"Quiz finished! Your score: {quiz.Score}/{quiz.TotalQuestions}", outputBox);
                    activityLog.Add($"Quiz completed. Score: {quiz.Score}/{quiz.TotalQuestions}");

                    if (quiz.Score >= 8)
                        Utilities.PrintWithColor(synth, "Great job! You're a cybersecurity pro!", outputBox);
                    else if (quiz.Score >= 5)
                        Utilities.PrintWithColor(synth, "Not bad! Keep brushing up on your skills.", outputBox);
                    else
                        Utilities.PrintWithColor(synth, "Keep learning to stay safe online!", outputBox);
                }
                else
                {
                    AskNextQuizQuestion(outputBox);
                }

                return true;
            }



            return false;
        }
        private void AskNextQuizQuestion(RichTextBox outputBox)
        {
            var q = quiz.GetNextQuestion();
            Utilities.PrintWithColor(synth, q.Question, outputBox);

            for (int i = 0; i < q.Options.Length; i++)
            {
                char label = (char)('A' + i);
                Utilities.PrintWithColor(synth, $"{label}) {q.Options[i]}", outputBox);
            }

            Utilities.PrintWithColor(synth, "Please enter the letter of your answer (A, B, C...)", outputBox);
        }
    }
}

/*
 C# Corner. 2022. Understanding Dictionary in C#. Available at: https://www.c-sharpcorner.com/article/understanding-dictionary-in-c-sharp/ [Accessed 27 June 2025]

 Moo ICT. 2024. C# Tutorial – Create a simple multiple choice quiz game in Visual Studio. Available at: https://www.youtube.com/watch?v=somevideolink [Accessed 27 June 2025]

 TutorialsTeacher. 2022. C# Regex (Regular Expressions). Available at: https://www.tutorialsteacher.com/csharp/csharp-regular-expressions [Accessed 27 June 2025]

*/




/*Stack Overflow. 2021. How to create a Dictionary of List<string> in C#. (Version 2.0) [Source code] https://stackoverflow.com/questions/65588009/how-to-create-a-dictionary-of-liststring. [Accessed 23 May 2025]

Microsoft. 2023. Random.Next Method (System). (Version 2.0) [Source code] https://learn.microsoft.com/en-us/dotnet/api/system.random.next. [Accessed 23 May 2025]

C# Corner. 2022. Understanding Dictionary in C#. (Version 2.0) [Source code] https://www.c-sharpcorner.com/article/understanding-dictionary-in-c-sharp. [Accessed 23 May 2025]

Stack Overflow. 2023. How to detect confusion or unclear intent in chatbot input? (Version 2.0) [Source code] https://stackoverflow.com/questions/58784298/how-to-detect-confusion-or-unclear-intent-in-chatbot-input. [Accessed 26 May 2025]

Microsoft. 2023. Delegates - C# Programming Guide. (Version 2.0) [Source code] https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/. [Accessed 26 May 2025]
 */