using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;

namespace PresentationLayer
{
    public class Program
    {
        static BL businessLayer = new BL();
        static int teacherID = 0;

        public static void Main(string[] args)
        {
            DisplayLoginMenu();
            Console.ReadLine();
        }

        private static List<string> LoginMenuOptions
        {
            get 
            {
                List<string> options = new List<string>() { "Login", "Exit" };
                return options;
            }
        }

        private static void DisplayLoginMenu()
        {
            List<string> loginMenuOptions = LoginMenuOptions;
            bool isInputFormatCorrect = false;
            int choice = 0;
            do
            {
                Console.WriteLine("Main Menu\n=========");
                for (int optionPos = 0; optionPos < loginMenuOptions.Count; optionPos++)
                    Console.WriteLine($"{optionPos + 1}. {loginMenuOptions[optionPos]}");
                Console.Write("Enter choice: ");
                isInputFormatCorrect = int.TryParse(Console.ReadLine(), out choice);
                if (isInputFormatCorrect)
                {
                    switch (choice)
                    {
                        case 1:
                            Login();
                            break;
                        case 2:
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice!");
                            Console.ReadKey();
                            ClearConsole();
                            break;
                    }
                }
                else
                    Console.WriteLine("Incorrect input format! Please try again!");
            }
            while (choice != 2 || !isInputFormatCorrect);
        }

        private static void Login()
        {
            ClearConsole();
            Console.WriteLine("Login\n=====");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            if (businessLayer.VerifyIfTeacherUsernameExists(username))
            {
                Console.Write("Password: ");
                string password = Console.ReadLine();
                if(businessLayer.VerifyIfTeacherPasswordIsCorrect(username, password))
                {
                    Console.WriteLine("Correct credentials, logging you in!");
                    teacherID = businessLayer.GetTeacherID(username, password);
                    DisplayTeacherMenu();
                }
                else
                {
                    Console.WriteLine("Incorrect credentials!");
                    Console.ReadKey();
                    ClearConsole();
                }
            }
            else
            {
                Console.WriteLine("The entered ID does not exist in our system.");
                Console.ReadKey();
                ClearConsole();
            }
        }

        private static List<string> TeacherMenuOptions
        {
            get
            {
                List<string> options = new List<string>() { "Add attendance", "Add a new group", "Add new student", "Add a new teacher", "Check a student's attendance percentage",
                                                            "Get all attendances submitted on a particular day", "Edit student" };
                return options;
            }
        }

        private static void DisplayTeacherMenu()
        {
            ClearConsole();
            List<string> teacherMenuOptions = TeacherMenuOptions;
            bool isInputFormatCorrect = false;
            int choice = 0;
            Console.WriteLine("Teacher's Menu\n==============");
            do
            {
                for (int optionPos = 0; optionPos < teacherMenuOptions.Count; optionPos++)
                    Console.WriteLine($"{optionPos + 1}. {teacherMenuOptions[optionPos]}");
                Console.Write("Enter choice: ");
                isInputFormatCorrect = int.TryParse(Console.ReadLine(), out choice);
                if (isInputFormatCorrect)
                {
                    switch (choice)
                    {
                        case 1:
                            AddAttendance();
                            break;
                        case 2:
                            AddNewGroup();
                            break;
                        case 3:
                            AddNewStudent();
                            break;
                        case 4:
                            AddNewTeacher();
                            break;
                        case 5:
                            CheckStudentAttendancePercentage();
                            break;
                        case 6:
                            GetAllAttendancesOnParticularDay();
                            break;
                        case 7:
                            EditStudent();
                            break;
                        default:
                            break;
                    }
                }
                else
                    Console.WriteLine("Incorrect input format! Please try again!");
            }
            while (choice != 2 || !isInputFormatCorrect);
        }

        private static void AddAttendance()
        {

        }

        private static void AddNewGroup()
        {

        }

        private static void AddStudent() 
        {

        }

        private static void AddNewTeacher()
        {

        }

        private static void CheckStudentAttendancePercentage()
        {

        }

        private static void GetAllAttendancesOnParticularDay()
        {

        }

        private static void EditStudent()
        {

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

        private static void ClearConsole()
        {
            Console.Clear();
        }
    }
}