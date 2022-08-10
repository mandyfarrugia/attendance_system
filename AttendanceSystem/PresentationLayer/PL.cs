using BusinessLayer;
using Domain;
using System;
using System.Collections.Generic;

namespace PresentationLayer
{
    public class PL
    {
        static readonly BL businessLayer = new BL();
        static int teacherID;
        static int choice;
        static int groupToSelect;
        static int studentToSelect;
        static int courseToSelect;
        static bool inputFormatMatch;
        static bool doesEmailExist;
        static bool isFieldEmpty;

        public static void Main(string[] args)
        {
            DisplayLoginMenu();
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
            inputFormatMatch = false;
            do
            {
                ClearConsole();
                DisplayTitle("Main Menu");
                for (int optionPos = 0; optionPos < loginMenuOptions.Count; optionPos++)
                    Console.WriteLine($"{optionPos + 1}. {loginMenuOptions[optionPos]}");
                Console.Write("Enter choice: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out choice);
                if (inputFormatMatch)
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
                            DisplayMessage("Goodbye!", true);
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
            while (choice != 3 || !inputFormatMatch);
        }

        private static void Login()
        {
            ClearConsole();
            DisplayTitle("Login");
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
            do
            {
                ClearConsole();
                DisplayTitle("Teacher's Menu");
                for (int optionPos = 0; optionPos < teacherMenuOptions.Count; optionPos++)
                    Console.WriteLine($"{optionPos + 1}. {teacherMenuOptions[optionPos]}");
                Console.Write("Enter choice: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out choice);
                if (inputFormatMatch)
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
                            DisplayMessage("Invalid choice!", MessageType.Error, true);
                            ClearConsole();
                            break;
                    }
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (choice != 10 || !inputFormatMatch);
        }

        private static void AddAttendance()
        {
            ClearConsole();
            inputFormatMatch = false;
            DisplayTitle("Add New Attendance");
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
                    DateTime dateOfToday = DateTime.Now;
                    businessLayer.AddNewLesson(groupToSelect, dateOfToday, teacherID);

                    Console.WriteLine("\nStudent ID\tStudent Name\t\tStudent Surname\t\tPresence (P/A)\n==========\t============\t\t===============\t\t==============");
                    List<Student> students = businessLayer.GetAllStudentsFromGroup(groupToSelect);
                    foreach (Student student in students)
                    {
                        hasAttendanceBeenTaken = false;
                        do
                        {
                            string attendanceRow = $"{student.StudentID}\t\t";
                            if(student.Name.Length >= 6)
                                attendanceRow += $"{student.Name}\t\t\t";
                            else
                                attendanceRow += $"{student.Name}\t\t\t";
                            if (student.Surname.Length <= 6)
                                attendanceRow += $"{student.Surname}\t\t\t\t";
                            else
                                attendanceRow += $"{student.Surname}\t\t\t";
                            Console.Write(attendanceRow);

                            inputFormatMatch = char.TryParse(Console.ReadLine(), out char presence);
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
            DisplayTitle("Add New Group");
            string groupName;
            bool doesGroupNameExist;
            do
            {
                Console.Write("Group Name: ");
                groupName = Console.ReadLine();
                isFieldEmpty = groupName.Equals(string.Empty);
                doesGroupNameExist = businessLayer.VerifyIfGroupExists(groupName);
                if (isFieldEmpty)
                    DisplayMessage("Group name cannot be empty!", MessageType.Error, false);
                else if (doesGroupNameExist)
                    DisplayMessage("Group name already exists!", MessageType.Error, false);
            }
            while (isFieldEmpty || doesGroupNameExist);

            List<Course> courses = businessLayer.GetAllCourses();
            foreach(Course course in courses)
                Console.WriteLine($"{course.CourseID}. {course.CourseTitle}");
            bool doesCourseExist = false;
            do
            {
                Console.Write("Select course: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out courseToSelect);
                if (inputFormatMatch)
                {
                    doesCourseExist = businessLayer.VerifyIfCourseExistsById(courseToSelect);
                    if (doesCourseExist)
                    {
                        businessLayer.AddNewGroup(groupName, courseToSelect);
                        DisplayMessage("Group added successfully!", MessageType.Success, true);
                    }
                    else
                        DisplayMessage("Could not find course with said ID!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!doesCourseExist || !inputFormatMatch);
        }

        private static void AddNewCourse()
        {
            ClearConsole();
            DisplayTitle("Add New Course");
            string courseTitle;
            do
            {
                Console.Write("Course Title: ");
                courseTitle = Console.ReadLine();
                isFieldEmpty = courseTitle.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Course title cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);
            businessLayer.AddNewCourse(courseTitle);
            DisplayMessage("Course added successfully!", MessageType.Success, true);
        }

        private static void AddNewStudent() 
        {
            ClearConsole();
            DisplayTitle("Add New Student");
            string name;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                isFieldEmpty = name.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Name cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);

            string surname;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();
                isFieldEmpty = surname.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                isFieldEmpty = email.Equals(string.Empty);
                doesEmailExist = businessLayer.VerifyIfStudentEmailExists(email);
                if (isFieldEmpty)
                    DisplayMessage("Email cannot be empty!", MessageType.Error, false);
                else if (doesEmailExist)
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (isFieldEmpty || doesEmailExist);

            bool doesGroupExist = false;
            Console.WriteLine(businessLayer.DisplayAllGroups());
            do
            {
                Console.Write("Select a group: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out groupToSelect);
                if(inputFormatMatch)
                {
                    doesGroupExist = businessLayer.VerifyIfGroupExists(groupToSelect);
                    if (!doesGroupExist)
                        DisplayMessage("Group does not exist!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !doesGroupExist);

            businessLayer.AddNewStudent(name, surname, email, groupToSelect);
            DisplayMessage("Student added successfully!", MessageType.Success, true);
        }

        private static void AddNewTeacher()
        {
            ClearConsole();
            DisplayTitle("Add New Teacher");
            string username;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                isFieldEmpty = username.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Username cannot be empty!", MessageType.Error, false);
                else if (businessLayer.VerifyIfTeacherUsernameExists(username))
                    DisplayMessage("Username already exists!", MessageType.Error, false);
            }
            while (isFieldEmpty || businessLayer.VerifyIfTeacherUsernameExists(username));

            string password;
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
                isFieldEmpty = password.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Password cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);

            string name;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                isFieldEmpty = name.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Name cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);

            string surname;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();
                isFieldEmpty = surname.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
            }
            while (isFieldEmpty);

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                isFieldEmpty = email.Equals(string.Empty);
                if (isFieldEmpty)
                    DisplayMessage("Surname cannot be empty!", MessageType.Error, false);
                else if (businessLayer.VerifyIfTeacherEmailExists(email))
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (isFieldEmpty || businessLayer.VerifyIfTeacherEmailExists(email));

            businessLayer.AddNewTeacher(username, password, name, surname, email);
            DisplayMessage("Teacher added successfully!", MessageType.Success, true);
        }

        private static void CheckStudentAttendancePercentage()
        {
            ClearConsole();
            int studentID;
            DisplayTitle("Attendance Percentage");
            foreach (Student student in businessLayer.GetAllStudents())
                Console.WriteLine($"{student.StudentID}. {student.Name} {student.Surname}");
            bool doesStudentExist = false;
            do
            {
                Console.Write("Choose a student: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out studentID);
                if (inputFormatMatch)
                {
                    doesStudentExist = businessLayer.VerifyIfStudentExists(studentID);
                    if (!doesStudentExist)
                        DisplayMessage("Student does not exist!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format", MessageType.Error, true);
            } 
            while (!inputFormatMatch || !doesStudentExist);

            string attendancePercentageResult = businessLayer.DisplayAttendancePercentageByStudentID(studentID);
            if (attendancePercentageResult.Contains("no attendance records"))
                DisplayMessage(attendancePercentageResult, MessageType.Error, true);
            else
                DisplayMessage(attendancePercentageResult, true);
        }

        private static void GetAllAttendancesOnParticularDay()
        {
            try
            {
                ClearConsole();
                int day, month, year;

                DisplayTitle("Submitted Attendances");
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
                        result += (attendancesOnParticularDayCount == 1) ? "only one attendance " : $"{attendancesOnParticularDayCount} attendances ";
                    else
                        result += "no attendances ";
                    result += $"on { dateStart.ToShortDateString()}.";
                    DisplayMessage(result, true);
                }
            }
            catch(ArgumentException)
            {
                DisplayMessage("Invalid date!", MessageType.Error, true);
            }
        }

        private static void EditStudent()
        {
            ClearConsole();
            DisplayTitle("Edit Student");
            string updates = string.Empty;
            bool doesStudentExist = false;
            Console.WriteLine(businessLayer.DisplayAllStudents());
            do
            {
                Console.Write("Choose a student: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out studentToSelect);
                if(inputFormatMatch)
                {
                    doesStudentExist = businessLayer.VerifyIfStudentExists(studentToSelect);
                    if (!doesStudentExist)
                        DisplayMessage("The entered ID does not exist in our system!", MessageType.Error, true);
                }
                else
                    DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !doesStudentExist);

            DisplayMessage("\nIf you do not want to change a field, press ENTER to skip.\n", MessageType.Warning, false);

            string name;
            Console.Write("Name: ");
            name = Console.ReadLine();

            string surname;
            Console.Write("Surname: ");
            surname = Console.ReadLine();

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                doesEmailExist = businessLayer.VerifyIfStudentEmailExists(email);
                if (doesEmailExist)
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (doesEmailExist);

            bool isConsentCorrect;
            List<Group> groups = businessLayer.GetAllGroups();
            foreach (Group group in groups)
                Console.WriteLine($"{group.GroupID}. {group.Name}");
            char groupUpdateConsent;
            do
            {
                Console.Write("Would you like to change the class group?: ");
                inputFormatMatch = char.TryParse(Console.ReadLine(), out groupUpdateConsent);
                isConsentCorrect = (groupUpdateConsent.Equals('Y') || groupUpdateConsent.Equals('y')) && !(groupUpdateConsent.Equals('N') || groupUpdateConsent.Equals('n'));
                if (!inputFormatMatch)
                    DisplayMessage("Incorrect input format!", MessageType.Error, true);
                else if (!isConsentCorrect)
                    DisplayMessage("You must enter either Y/y (Yes) or N/n!", MessageType.Error, true);
            }
            while (!inputFormatMatch || !isConsentCorrect);

            bool doesGroupExist = false;
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
                            doesGroupExist = businessLayer.VerifyIfGroupExists(groupToSelect);
                            if (!doesGroupExist)
                                DisplayMessage("Group does not exist!", MessageType.Error, true);
                        }
                        else
                            DisplayMessage("Incorrect input format! Please try again!", MessageType.Error, true);
                    }
                    while (!inputFormatMatch || !doesGroupExist);
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
            DisplayTitle("Edit Teacher");
            DisplayMessage("If you do not want to change a field, press ENTER to skip.\n", MessageType.Warning, false);

            bool doesTeacherUsernameExist;
            string username;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                doesTeacherUsernameExist = businessLayer.VerifyIfTeacherUsernameExists(username);
                if (doesTeacherUsernameExist)
                    DisplayMessage("Username already exists!", MessageType.Error, false);
            }
            while (doesTeacherUsernameExist);

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Surname: ");
            string surname = Console.ReadLine();

            bool doesTeacherEmailExist;
            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                doesTeacherEmailExist = businessLayer.VerifyIfTeacherEmailExists(email);
                if (doesTeacherEmailExist)
                    DisplayMessage("Email already exists!", MessageType.Error, false);
            }
            while (doesTeacherEmailExist);

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
                    foregroundColour = ConsoleColor.DarkGreen;
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

        private static void DisplayTitle(string title)
        {
            DisplayMessage(title, false);
            for (int titlePos = 0; titlePos < title.Length; titlePos++)
            {
                Console.Write("=");
                if (titlePos == title.Length - 1)
                    Console.Write("\n");
            }
        }

        private static void ClearConsole()
        {
            Console.Clear();
        }
    }
}