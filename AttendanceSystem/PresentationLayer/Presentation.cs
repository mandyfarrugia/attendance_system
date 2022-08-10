using System;

namespace PresentationLayer
{
    public class Presentation
    {
        public enum MessageType { Warning, Error, Success }

        public static void ChangeForegroundColour(MessageType messageType)
        {
            ConsoleColor? foregroundColour = null;
            switch (messageType)
            {
                case MessageType.Error:
                    foregroundColour = ConsoleColor.Red;
                    break;
                case MessageType.Success:
                    foregroundColour = ConsoleColor.DarkGreen;
                    break;
                case MessageType.Warning:
                    foregroundColour = ConsoleColor.DarkYellow;
                    break;
            }
            Console.ForegroundColor = (ConsoleColor)foregroundColour;
        }

        public static void DisplayMessage(string message, bool promptKeyPress)
        {
            Console.WriteLine(message);
            if (promptKeyPress)
                Console.ReadKey();
        }

        public static void DisplayMessage(string message, MessageType messageType, bool promptKeyPress)
        {
            ChangeForegroundColour(messageType);
            DisplayMessage(message, promptKeyPress);
            Console.ResetColor();
        }

        public static void DisplayTitle(string title)
        {
            string underline = string.Empty;
            DisplayMessage(title, false);
            for (int titlePos = 0; titlePos < title.Length; titlePos++)
            {
                underline += "=";
                if (titlePos == title.Length - 1)
                    underline += "\n";
            }
            Console.Write(underline);
        }

        public static void ClearConsole()
        {
            Console.Clear();
        }
    }
}
