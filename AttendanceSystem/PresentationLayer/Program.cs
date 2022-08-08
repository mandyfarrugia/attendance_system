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
                List<string> options = new List<string>() { "Login", "Register", "Exit" };
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
                ClearConsole();
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
                            AddNewTeacher();
                            break;
                        case 3:
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
            while (choice != 3 || !isInputFormatCorrect);
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
                List<string> options = new List<string>() { "Add attendance", "Add a new group", "Add course", "Add new student", "Add a new teacher", "Check a student's attendance percentage",
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
            do
            {
                ClearConsole();
                Console.WriteLine("Teacher's Menu\n==============");
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
                            AddNewCourse();
                            break;
                        case 4:
                            AddNewStudent();
                            break;
                        case 5:
                            AddNewTeacher();
                            break;
                        case 6:
                            CheckStudentAttendancePercentage();
                            break;
                        case 7:
                            GetAllAttendancesOnParticularDay();
                            break;
                        case 8:
                            EditStudent();
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

        private static void AddAttendance()
        {

        }

        private static void AddNewGroup()
        {
            ClearConsole();
            Console.WriteLine("Add New Group\n=============");
            string groupName = string.Empty;
            do
            {
                Console.Write("Group Name: ");
                groupName = Console.ReadLine();
                if (groupName.Equals(string.Empty))
                    DisplayMessage("Group name cannot be empty!", MessageType.Error);
            }
            while (groupName.Equals(string.Empty));
        }

        private static void AddNewCourse()
        {
            ClearConsole();
            Console.WriteLine("Add New Course\n==============");
            string courseTitle = string.Empty;
            do
            {
                Console.Write("Course Title: ");
                courseTitle = Console.ReadLine();
                if (courseTitle.Equals(string.Empty))
                    DisplayMessage("Course title cannot be empty!", MessageType.Error);
            }
            while (courseTitle.Equals(string.Empty));
            //businessLayer.AddNewCourse(courseTitle);
        }

        private static void AddNewStudent() 
        {

        }

        private static void AddNewTeacher()
        {
            ClearConsole();
            Console.WriteLine("Add New Teacher\n===============");

            string username = string.Empty;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();

                if (username.Equals(string.Empty))
                    DisplayMessage("Username cannot be empty!", MessageType.Error);
                else if (businessLayer.VerifyIfTeacherUsernameExists(username))
                    DisplayMessage("Username already exists!", MessageType.Error);
            }
            while (username.Equals(string.Empty) || businessLayer.VerifyIfTeacherUsernameExists(username));

            string password = string.Empty;
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();

                if (password.Equals(string.Empty))
                    DisplayMessage("Password cannot be empty!", MessageType.Error);
            }
            while (password.Equals(string.Empty));

            string name = string.Empty;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();

                if (name.Equals(string.Empty))
                    DisplayMessage("Name cannot be empty!", MessageType.Error);
            }
            while (name.Equals(string.Empty));

            string surname = string.Empty;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();

                if (surname.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error);
            }
            while (surname.Equals(string.Empty));

            string email = string.Empty;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                if (email.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error);
                else if (businessLayer.VerifyIfTeacherEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error);
            }
            while (email.Equals(string.Empty) || businessLayer.VerifyIfTeacherEmailExists(email));

            businessLayer.AddNewTeacher(username, password, name, surname, email);
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
            Console.ResetColor();
        }

        private static void ClearConsole()
        {
            Console.Clear();
        }
    }
}