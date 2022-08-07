using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayLoginMenu();
            Console.ReadLine();
        }

        private static List<string> MenuOptions
        {
            get 
            {
                List<string> options = new List<string>() { "Login", "Exit" };
                return options;
            }
        }

        private static void DisplayLoginMenu()
        {
            List<string> loginMenuOptions = MenuOptions;
            bool isInputFormatCorrect = false;
            int choice = 0;
            Console.WriteLine("Main Menu\n=========");
            do
            {
                for (int optionPos = 0; optionPos < loginMenuOptions.Count; optionPos++)
                    Console.WriteLine($"{optionPos + 1}. {loginMenuOptions[optionPos]}");
                Console.Write("Enter choice: ");
                isInputFormatCorrect = int.TryParse(Console.ReadLine(), out choice);
                if (isInputFormatCorrect)
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("You have selected the first option!");
                            break;
                        case 2:
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                else
                    Console.WriteLine("Incorrect input format! Please try again!");
            }
            while (choice != 2 || !isInputFormatCorrect);
        }

        private enum MessageType { Warning, Error, Success }

        private static void ChangeForegroundColour(MessageType messageType)
        {
            ConsoleColor? foregroundColour = null;
            switch (messageType)
            {
                case MessageType.Error:
                    foregroundColour = ConsoleColor.Red;
                    break;
                case MessageType.Success:
                    foregroundColour = ConsoleColor.Green;
                    break;
                case MessageType.Warning:
                    foregroundColour = ConsoleColor.DarkYellow;
                    break;
            }
            Console.ForegroundColor = (ConsoleColor)foregroundColour;
        }

        private static void DisplayMessage(string message, MessageType messageType)
        {
            ChangeForegroundColour(messageType);
            Console.WriteLine(message);
        }
    }
}