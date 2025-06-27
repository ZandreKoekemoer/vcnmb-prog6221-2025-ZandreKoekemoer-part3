using System;

namespace CyberBotGUI.Bot
{
    // Reference C# Corner. Working With Classes and Objects in C#.

    // According to C# Corner (2020), you can create custom classes with fields and methods to represent real-world objects.
    // I used a custom TaskItem class to store task title, description, status, and reminder, and added them to a List<TaskItem>.
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem(string title, string description, DateTime? reminderDate = null)
        {
            Title = title;
            Description = description;
            ReminderDate = reminderDate;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[Completed]" : "[Pending]";
            string reminder = ReminderDate.HasValue ? $" (Reminder: {ReminderDate.Value.ToShortDateString()})" : "";
            return $"{status} {Title} - {Description}{reminder}";
        }
    }
}
/*
 C# Corner. 2020. Working With Classes and Objects in C#. Available at: https://www.c-sharpcorner.com/article/working-with-classes-and-objects-in-c-sharp/ [Accessed 27 June 2025]
*/