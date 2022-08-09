using BusinessLayer;
using Domain;
using System;
using System.Collections.Generic;

namespace PresentationLayer
{
    public class PL
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
                            DisplayMessage("Invalid choice!", MessageType.Error, true);
                            ClearConsole();
                            break;
                    }
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
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
                    DisplayMessage("Correct credentials, logging you in!", MessageType.Success, true);
                    teacherID = businessLayer.GetTeacherID(username, password);
                    DisplayTeacherMenu();
                }
                else
                {
                    DisplayMessage("Incorrect credentials!", MessageType.Error, true);
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
                List<string> options = new List<string>() { "Add attendance", "Add a new group", "Add a new course", "Add new student", "Add a new teacher", "Check a student's attendance percentage",
                                                            "Get all attendances submitted on a particular day", "Edit student", "Edit teacher", "Logout" };
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
                        case 9:
                            EditTeacher();
                            break;
                        case 10:
                            Logout();
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
            while (choice != 10 || !isInputFormatCorrect);
        }

        private static void AddAttendance()
        {
            ClearConsole();
            int groupToSelect = 0;
            bool inputFormatMatch = false;
            Console.WriteLine("New Attendance\n==============");
            string groups = businessLayer.DisplayAllGroups();
            if (!groups.Contains("No groups"))
            {
                Console.WriteLine(groups);
                do
                {
                    Console.Write("Select a group: ");
                    inputFormatMatch = int.TryParse(Console.ReadLine(), out groupToSelect);
                    if (!inputFormatMatch)
                        DisplayMessage("Incorrect input format!", MessageType.Error, false);
                    else if (!businessLayer.VerifyIfGroupExists(groupToSelect))
                        DisplayMessage("Cannot find group with said ID!", MessageType.Error, false);
                }
                while (!inputFormatMatch || !businessLayer.VerifyIfGroupExists(groupToSelect));

                if (businessLayer.VerifyIfGroupHasStudents(groupToSelect))
                {
                    bool hasAttendanceBeenTaken;
                    char presence;
                    DateTime dateOfToday = DateTime.Now;
                    businessLayer.AddNewLesson(groupToSelect, dateOfToday, teacherID);

                    Console.WriteLine("\nStudent ID\tStudent Name\tStudent Surname\t\t\tPresence (P/A)\n==========\t============\t===============\t\t==============");
                    List<Student> students = businessLayer.GetAllStudentsFromGroup(groupToSelect);
                    foreach (Student student in students)
                    {
                        hasAttendanceBeenTaken = false;
                        do
                        {
                            Console.Write($"{student.StudentID}\t\t{student.Name}\t\t{student.Surname}\t\t");
                            inputFormatMatch = char.TryParse(Console.ReadLine(), out presence);
                            if (inputFormatMatch)
                            {
                                if (presence.Equals('p') || presence.Equals('P'))
                                {
                                    businessLayer.AddNewStudentAttendance(student.StudentID, true);
                                    hasAttendanceBeenTaken = true;
                                }
                                else if (presence.Equals('a') || presence.Equals('A'))
                                {
                                    businessLayer.AddNewStudentAttendance(student.StudentID, false);
                                    hasAttendanceBeenTaken = true;
                                }
                                else
                                    DisplayMessage("You must only enter P/p or A/a!", MessageType.Error, true);
                            }
                            else
                                DisplayMessage("Incorrect input format!", MessageType.Error, false);
                        }
                        while (!hasAttendanceBeenTaken);
                    }
                }
                else
                    DisplayMessage("There are no students in this group!", MessageType.Error, true);
            }
            else
                DisplayMessage(groups, MessageType.Error, true);
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
                    DisplayMessage("Group name cannot be empty!", MessageType.Error, false);
            }
            while (groupName.Equals(string.Empty));

            int courseToSelect = 0;
            List<Course> courses = businessLayer.GetAllCourses();
            foreach(Course course in courses)
                Console.WriteLine($"{course.CourseID}. {course.CourseTitle}");
            bool inputFormatMatch = false;
            do
            {
                Console.Write("Select course: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out courseToSelect);
                if (inputFormatMatch)
                {
                    if (businessLayer.VerifyIfCourseExistsById(courseToSelect))
                        businessLayer.AddNewGroup(groupName, courseToSelect);
                    DisplayMessage("Group added successfully!", MessageType.Success, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!businessLayer.VerifyIfCourseExistsById(courseToSelect) || !inputFormatMatch);
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
                    DisplayMessage("Course title cannot be empty!", MessageType.Error, false);
            }
            while (courseTitle.Equals(string.Empty));
            businessLayer.AddNewCourse(courseTitle);
            DisplayMessage("Course added successfully!", MessageType.Success, true);
        }

        private static void AddNewStudent() 
        {
            ClearConsole();
            Console.WriteLine("Add New Student\n===============");

            string name = string.Empty;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();

                if (name.Equals(string.Empty))
                    DisplayMessage("Name cannot be empty!", MessageType.Error, false);
            }
            while (name.Equals(string.Empty));

            string surname = string.Empty;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();

                if (surname.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
            }
            while (surname.Equals(string.Empty));

            string email = string.Empty;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                if (email.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
                else if (businessLayer.VerifyIfStudentEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (email.Equals(string.Empty) || businessLayer.VerifyIfStudentEmailExists(email));

            int groupToSelect = 0;
            bool inputFormatMatch = false;
            List<Group> groups = businessLayer.GetAllGroups();
            foreach(Group group in groups)
                Console.WriteLine($"{group.GroupID}. {group.Name}");
            do
            {
                Console.Write("Select a group: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out groupToSelect);
                if(inputFormatMatch)
                {
                    if (!businessLayer.VerifyIfGroupExists(groupToSelect))
                        DisplayMessage("Group does not exist!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !businessLayer.VerifyIfGroupExists(groupToSelect));

            businessLayer.AddNewStudent(name, surname, email, groupToSelect);
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
                    DisplayMessage("Username cannot be empty!", MessageType.Error, false);
                else if (businessLayer.VerifyIfTeacherUsernameExists(username))
                    DisplayMessage("Username already exists!", MessageType.Error, false);
            }
            while (username.Equals(string.Empty) || businessLayer.VerifyIfTeacherUsernameExists(username));

            string password = string.Empty;
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();

                if (password.Equals(string.Empty))
                    DisplayMessage("Password cannot be empty!", MessageType.Error, false);
            }
            while (password.Equals(string.Empty));

            string name = string.Empty;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();

                if (name.Equals(string.Empty))
                    DisplayMessage("Name cannot be empty!", MessageType.Error, false);
            }
            while (name.Equals(string.Empty));

            string surname = string.Empty;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();

                if (surname.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
            }
            while (surname.Equals(string.Empty));

            string email = string.Empty;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                if (email.Equals(string.Empty))
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
                else if (businessLayer.VerifyIfTeacherEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (email.Equals(string.Empty) || businessLayer.VerifyIfTeacherEmailExists(email));

            businessLayer.AddNewTeacher(username, password, name, surname, email);
        }

        private static void CheckStudentAttendancePercentage()
        {
            ClearConsole();
            int studentID = 0;
            bool inputFormatMatch = false;
            Console.WriteLine("Attendance Percentage\n=====================");
            foreach (Student student in businessLayer.GetAllStudents())
                Console.WriteLine($"{student.StudentID}. {student.Name} {student.Surname}");
            do
            {
                Console.Write("Choose a student: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out studentID);
            } 
            while (!inputFormatMatch);

            string attendancePercentageResult = businessLayer.DisplayAttendancePercentageByStudentID(studentID);
            if(attendancePercentageResult.Contains("no attendance records"))
                DisplayMessage(attendancePercentageResult, MessageType.Error, true);
        }

        private static void GetAllAttendancesOnParticularDay()
        {
            ClearConsole();
            int day, month, year;
            bool inputFormatMatch = false;
            Console.WriteLine("Submitted Attendances\n=====================");

            do
            {
                Console.Write("Day: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out day);
                if (!inputFormatMatch)
                    DisplayMessage("Incorrect input format!", MessageType.Error, true);
            }
            while (!inputFormatMatch);

            do
            {
                Console.Write("Month: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out month);
                if (!inputFormatMatch)
                    DisplayMessage("Incorrect input format!", MessageType.Error, true);
            }
            while (!inputFormatMatch);

            do
            {
                Console.Write("Year: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out year);
                if (!inputFormatMatch)
                    DisplayMessage("Incorrect input format!", MessageType.Error, true);
            }
            while (!inputFormatMatch);

            DateTime dateStart = new DateTime(year, month, day);
            if (dateStart > DateTime.Now)
            {
                DisplayMessage("Date cannot be future date!", MessageType.Error, true);
                return;
            }
            else
            {
                DateTime dateEnd = dateStart.AddDays(1);
                int attendancesOnParticularDayCount = businessLayer.GetAllAttendancesOnParticularDay(teacherID, dateStart, dateEnd);
                string result = $"You have submitted ";
                if (attendancesOnParticularDayCount != 0)
                    result += (attendancesOnParticularDayCount == 1) ? "only one attendance " : $"{attendancesOnParticularDayCount} attendances";
                else
                    result += "no attendances ";
                result += $"on { dateStart.ToShortDateString()}.";
                DisplayMessage(result, true);
            }
        }

        private static void EditStudent()
        {
            ClearConsole();
            Console.WriteLine("Edit Teacher\n============");
            string updates = string.Empty;
            bool inputFormatMatch = false;
            int studentToSelect = 0;
            foreach (Student student in businessLayer.GetAllStudents())
                Console.WriteLine($"{student.StudentID}. {student.Name} {student.Surname}");
            do
            {
                Console.Write("Choose a student: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out studentToSelect);
                if(inputFormatMatch)
                {
                    if (!businessLayer.VerifyIfStudentExists(studentToSelect))
                        DisplayMessage("The entered ID does not exist in our system!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !businessLayer.VerifyIfStudentExists(studentToSelect));

            DisplayMessage("\nIf you do not want to change a field, press ENTER to skip.\n", MessageType.Warning, false);

            string name = string.Empty;
            Console.Write("Name: ");
            name = Console.ReadLine();

            string surname = string.Empty;
            Console.Write("Surname: ");
            surname = Console.ReadLine();

            string email = string.Empty;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                if (businessLayer.VerifyIfStudentEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (businessLayer.VerifyIfStudentEmailExists(email));

            int groupToSelect = 0;
            inputFormatMatch = false;
            List<Group> groups = businessLayer.GetAllGroups();
            foreach (Group group in groups)
                Console.WriteLine($"{group.GroupID}. {group.Name}");
            char groupUpdateConsent;
            do
            {
                Console.Write("Would you like to change the class group?: ");
                inputFormatMatch = char.TryParse(Console.ReadLine(), out groupUpdateConsent);
                if (!inputFormatMatch)
                    DisplayMessage("Incorrect input format!", MessageType.Error, true);
                else if (!(groupUpdateConsent.Equals('Y') || groupUpdateConsent.Equals('y')) && !(groupUpdateConsent.Equals('N') || groupUpdateConsent.Equals('n')))
                    DisplayMessage("You must enter either Y/y (Yes) or N/n!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !(groupUpdateConsent.Equals('Y') || groupUpdateConsent.Equals('y')) && !(groupUpdateConsent.Equals('N') || groupUpdateConsent.Equals('n')));

            if(inputFormatMatch)
            {
                if(groupUpdateConsent.Equals('y') || groupUpdateConsent.Equals('Y'))
                {
                    do
                    {
                        Console.Write("Select a group: ");
                        inputFormatMatch = int.TryParse(Console.ReadLine(), out groupToSelect);
                        if (inputFormatMatch)
                        {
                            if (!businessLayer.VerifyIfGroupExists(groupToSelect))
                                DisplayMessage("Group does not exist!", MessageType.Error, true);
                        }
                        else
                            DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
                    }
                    while (!inputFormatMatch || !businessLayer.VerifyIfGroupExists(groupToSelect));
                    updates = businessLayer.EditStudent(studentToSelect, name, surname, email, groupToSelect);
                }
                else if(groupUpdateConsent.Equals('n') || groupUpdateConsent.Equals('N'))
                    updates = businessLayer.EditStudent(studentToSelect, name, surname, email);
            }

            if (updates.Equals(string.Empty))
                DisplayMessage("No changes have been inflicted.", MessageType.Warning, true);
            else
                DisplayMessage(updates, MessageType.Success, true);
        }

        private static void EditTeacher()
        {
            ClearConsole();
            Console.WriteLine("Edit Teacher\n============");
            DisplayMessage("If you do not want to change a field, press ENTER to skip.\n", MessageType.Warning, false);

            string username = string.Empty;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                if (businessLayer.VerifyIfTeacherUsernameExists(username))
                    DisplayMessage("Username already exists!", MessageType.Error, false);
            }
            while (businessLayer.VerifyIfTeacherUsernameExists(username));

            string password = string.Empty;
            Console.Write("Password: ");
            password = Console.ReadLine();

            string name = string.Empty;
            Console.Write("Name: ");
            name = Console.ReadLine();

            string surname = string.Empty;
            Console.Write("Surname: ");
            surname = Console.ReadLine();

            string email = string.Empty;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                if (businessLayer.VerifyIfTeacherEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (businessLayer.VerifyIfTeacherEmailExists(email));

            string updates = businessLayer.EditTeacher(teacherID, username, password, name, surname, email);
            if (updates.Equals(string.Empty))
                DisplayMessage("No changes have been inflicted.", MessageType.Warning, true);
            else
                DisplayMessage(updates, MessageType.Success, true);
        }

        private static void Logout()
        {
            teacherID = 0;
            DisplayMessage("Logging you out...", MessageType.Warning, true);
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

        private static void DisplayMessage(string message, bool promptKeyPress)
        {
            Console.WriteLine(message);
            if (promptKeyPress)
                Console.ReadKey();
        }

        private static void DisplayMessage(string message, MessageType messageType, bool promptKeyPress)
        {
            ChangeForegroundColour(messageType);
            DisplayMessage(message, promptKeyPress);
            Console.ResetColor();
        }

        private static void ClearConsole()
        {
            Console.Clear();
        }
    }
}