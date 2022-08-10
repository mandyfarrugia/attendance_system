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
                Presentation.ClearConsole();
                Presentation.DisplayTitle("Main Menu");
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
                            Presentation.DisplayMessage("Goodbye!", true);
                            break;
                        default:
                            Presentation.DisplayMessage("Invalid choice!", Presentation.MessageType.Error, true);
                            Presentation.ClearConsole();
                            break;
                    }
                }
                else
                    Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
            }
            while (choice != 3 || !inputFormatMatch);
        }

        private static void Login()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Login");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            if (businessLayer.VerifyIfTeacherUsernameExists(username))
            {
                Console.Write("Password: ");
                string password = Console.ReadLine();
                if(businessLayer.VerifyIfTeacherPasswordIsCorrect(username, password))
                {
                    Presentation.DisplayMessage("Correct credentials, logging you in!", Presentation.MessageType.Success, true);
                    teacherID = businessLayer.GetTeacherID(username, password);
                    DisplayTeacherMenu();
                }
                else
                {
                    Presentation.DisplayMessage("Incorrect credentials!", Presentation.MessageType.Error, true);
                    Presentation.ClearConsole();
                }
            }
            else
            {
                Console.WriteLine("The entered ID does not exist in our system.");
                Console.ReadKey();
                Presentation.ClearConsole();
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
            Presentation.ClearConsole();
            List<string> teacherMenuOptions = TeacherMenuOptions;
            do
            {
                Presentation.ClearConsole();
                Presentation.DisplayTitle("Teacher's Menu");
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
                            Presentation.DisplayMessage("Invalid choice!", Presentation.MessageType.Error, true);
                            Presentation.ClearConsole();
                            break;
                    }
                }
                else
                    Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
            }
            while (choice != 10 || !inputFormatMatch);
        }

        private static void AddAttendance()
        {
            Presentation.ClearConsole();
            inputFormatMatch = false;
            Presentation.DisplayTitle("Add New Attendance");
            string groups = businessLayer.DisplayAllGroups();
            if (!groups.Contains("No groups"))
            {
                bool doesGroupExist;
                Console.WriteLine(groups);
                do
                {
                    Console.Write("Select a group: ");
                    inputFormatMatch = int.TryParse(Console.ReadLine(), out groupToSelect);
                    doesGroupExist = businessLayer.VerifyIfGroupExists(groupToSelect);
                    if (!inputFormatMatch)
                        Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, false);
                    else if (!doesGroupExist)
                        Presentation.DisplayMessage("Cannot find group with said ID!", Presentation.MessageType.Error, false);
                }
                while (!inputFormatMatch || !doesGroupExist);

                if (businessLayer.VerifyIfGroupHasStudents(groupToSelect))
                {
                    bool hasAttendanceBeenTaken;
                    DateTime dateOfToday = DateTime.Now;
                    businessLayer.AddNewLesson(groupToSelect, dateOfToday, teacherID);

                    Console.WriteLine("\nID\tStudent Name\t\tSurname\t\t\tPresence\n=================================================================");
                    List<Student> students = businessLayer.GetAllStudentsFromGroup(groupToSelect);
                    foreach (Student student in students)
                    {
                        hasAttendanceBeenTaken = false;
                        do
                        {
                            string attendanceRow = $"{student.StudentID}\t{student.Name}\t\t\t{student.Surname}\t\t\t";
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
                                    Presentation.DisplayMessage("You must only enter P/p or A/a!", Presentation.MessageType.Error, true);
                            }
                            else
                                Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, false);
                        }
                        while (!hasAttendanceBeenTaken);
                    }
                }
                else
                    Presentation.DisplayMessage("There are no students in this group!", Presentation.MessageType.Error, true);
            }
            else
                Presentation.DisplayMessage(groups, Presentation.MessageType.Error, true);
        }

        private static void AddNewGroup()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Add New Group");
            string groupName;
            bool doesGroupNameExist;
            do
            {
                Console.Write("Group Name: ");
                groupName = Console.ReadLine();
                isFieldEmpty = groupName.Equals(string.Empty);
                doesGroupNameExist = businessLayer.VerifyIfGroupExists(groupName);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Group name cannot be empty!", Presentation.MessageType.Error, false);
                else if (doesGroupNameExist)
                    Presentation.DisplayMessage("Group name already exists!", Presentation.MessageType.Error, false);
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
                        Presentation.DisplayMessage("Group added successfully!", Presentation.MessageType.Success, true);
                    }
                    else
                        Presentation.DisplayMessage("Could not find course with said ID!", Presentation.MessageType.Error, true);
                }
                else
                    Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
            }
            while (!doesCourseExist || !inputFormatMatch);
        }

        private static void AddNewCourse()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Add New Course");
            string courseTitle;
            do
            {
                Console.Write("Course Title: ");
                courseTitle = Console.ReadLine();
                isFieldEmpty = courseTitle.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Course title cannot be empty!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty);
            businessLayer.AddNewCourse(courseTitle);
            Presentation.DisplayMessage("Course added successfully!", Presentation.MessageType.Success, true);
        }

        private static void AddNewStudent() 
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Add New Student");
            string name;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                isFieldEmpty = name.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Name cannot be empty!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty);

            string surname;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();
                isFieldEmpty = surname.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Surname cannot be empty!", Presentation.MessageType.Error, false);
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
                    Presentation.DisplayMessage("Email cannot be empty!", Presentation.MessageType.Error, false);
                else if (doesEmailExist)
                    Presentation.DisplayMessage("Email already exists!", Presentation.MessageType.Error, false);
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
                        Presentation.DisplayMessage("Group does not exist!", Presentation.MessageType.Error, true);
                }
                else
                    Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
            }
            while (!inputFormatMatch || !doesGroupExist);

            businessLayer.AddNewStudent(name, surname, email, groupToSelect);
            Presentation.DisplayMessage("Student added successfully!", Presentation.MessageType.Success, true);
        }

        private static void AddNewTeacher()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Add New Teacher");
            string username;
            bool doesTeacherUsernameExist;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                isFieldEmpty = username.Equals(string.Empty);
                doesTeacherUsernameExist = businessLayer.VerifyIfTeacherUsernameExists(username);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Username cannot be empty!", Presentation.MessageType.Error, false);
                else if (doesTeacherUsernameExist)
                    Presentation.DisplayMessage("Username already exists!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty || doesTeacherUsernameExist);

            string password;
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
                isFieldEmpty = password.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Password cannot be empty!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty);

            string name;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                isFieldEmpty = name.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Name cannot be empty!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty);

            string surname;
            do
            {
                Console.Write("Surname: ");
                surname = Console.ReadLine();
                isFieldEmpty = surname.Equals(string.Empty);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Surname cannot be empty!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty);

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
                isFieldEmpty = email.Equals(string.Empty);
                doesEmailExist = businessLayer.VerifyIfTeacherEmailExists(email);
                if (isFieldEmpty)
                    Presentation.DisplayMessage("Email cannot be empty!", Presentation.MessageType.Error, false);
                else if (doesEmailExist)
                    Presentation.DisplayMessage("Email already exists!", Presentation.MessageType.Error, false);
            }
            while (isFieldEmpty || doesEmailExist);

            businessLayer.AddNewTeacher(username, password, name, surname, email);
            Presentation.DisplayMessage("Teacher added successfully!", Presentation.MessageType.Success, true);
        }

        private static void CheckStudentAttendancePercentage()
        {
            Presentation.ClearConsole();
            int studentID;
            Presentation.DisplayTitle("Attendance Percentage");
            Console.WriteLine(businessLayer.DisplayAllStudents());
            bool doesStudentExist = false;
            do
            {
                Console.Write("Choose a student: ");
                inputFormatMatch = int.TryParse(Console.ReadLine(), out studentID);
                if (inputFormatMatch)
                {
                    doesStudentExist = businessLayer.VerifyIfStudentExists(studentID);
                    if (!doesStudentExist)
                        Presentation.DisplayMessage("Student does not exist!", Presentation.MessageType.Error, true);
                }
                else
                    Presentation.DisplayMessage("Incorrect input format", Presentation.MessageType.Error, true);
            } 
            while (!inputFormatMatch || !doesStudentExist);

            string attendancePercentageResult = businessLayer.DisplayAttendancePercentageByStudentID(studentID);
            if (attendancePercentageResult.Contains("no attendance records"))
                Presentation.DisplayMessage(attendancePercentageResult, Presentation.MessageType.Error, true);
            else
                Presentation.DisplayMessage(attendancePercentageResult, true);
        }

        private static void GetAllAttendancesOnParticularDay()
        {
            try
            {
                Presentation.ClearConsole();
                int day, month, year;

                Presentation.DisplayTitle("Submitted Attendances");
                do
                {
                    Console.Write("Day: ");
                    inputFormatMatch = int.TryParse(Console.ReadLine(), out day);
                    if (!inputFormatMatch)
                        Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, true);
                }
                while (!inputFormatMatch);

                do
                {
                    Console.Write("Month: ");
                    inputFormatMatch = int.TryParse(Console.ReadLine(), out month);
                    if (!inputFormatMatch)
                        Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, true);
                }
                while (!inputFormatMatch);

                do
                {
                    Console.Write("Year: ");
                    inputFormatMatch = int.TryParse(Console.ReadLine(), out year);
                    if (!inputFormatMatch)
                        Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, true);
                }
                while (!inputFormatMatch);

                DateTime dateStart = new DateTime(year, month, day);
                if (dateStart > DateTime.Now)
                {
                    Presentation.DisplayMessage("Date cannot be future date!", Presentation.MessageType.Error, true);
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
                    Presentation.DisplayMessage(result, true);
                }
            }
            catch(ArgumentException)
            {
                Presentation.DisplayMessage("Invalid date!", Presentation.MessageType.Error, true);
            }
        }

        private static void EditStudent()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Edit Student");
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
                        Presentation.DisplayMessage("The entered ID does not exist in our system!", Presentation.MessageType.Error, true);
                }
                else
                    Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
            }
            while (!inputFormatMatch || !doesStudentExist);

            Presentation.DisplayMessage("\nIf you do not want to change a field, press ENTER to skip.\n", Presentation.MessageType.Warning, false);

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
                    Presentation.DisplayMessage("Email already exists!", Presentation.MessageType.Error, false);
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
                    Presentation.DisplayMessage("Incorrect input format!", Presentation.MessageType.Error, true);
                else if (!isConsentCorrect)
                    Presentation.DisplayMessage("You must enter either Y/y (Yes) or N/n!", Presentation.MessageType.Error, true);
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
                                Presentation.DisplayMessage("Group does not exist!", Presentation.MessageType.Error, true);
                        }
                        else
                            Presentation.DisplayMessage("Incorrect input format! Please try again!", Presentation.MessageType.Error, true);
                    }
                    while (!inputFormatMatch || !doesGroupExist);
                    updates = businessLayer.EditStudent(studentToSelect, name, surname, email, groupToSelect);
                }
                else if(groupUpdateConsent.Equals('n') || groupUpdateConsent.Equals('N'))
                    updates = businessLayer.EditStudent(studentToSelect, name, surname, email);
            }

            if (updates.Equals(string.Empty))
                Presentation.DisplayMessage("No changes have been inflicted.", Presentation.MessageType.Warning, true);
            else
                Presentation.DisplayMessage(updates, Presentation.MessageType.Success, true);
        }

        private static void EditTeacher()
        {
            Presentation.ClearConsole();
            Presentation.DisplayTitle("Edit Teacher");
            Presentation.DisplayMessage("If you do not want to change a field, press ENTER to skip.\n", Presentation.MessageType.Warning, false);

            bool doesTeacherUsernameExist;
            string username;
            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine();
                doesTeacherUsernameExist = businessLayer.VerifyIfTeacherUsernameExists(username);
                if (doesTeacherUsernameExist)
                    Presentation.DisplayMessage("Username already exists!", Presentation.MessageType.Error, false);
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
                    Presentation.DisplayMessage("Email already exists!", Presentation.MessageType.Error, false);
            }
            while (doesTeacherEmailExist);

            string updates = businessLayer.EditTeacher(teacherID, username, password, name, surname, email);
            if (updates.Equals(string.Empty))
                Presentation.DisplayMessage("No changes have been inflicted.", Presentation.MessageType.Warning, true);
            else
                Presentation.DisplayMessage(updates, Presentation.MessageType.Success, true);
        }

        private static void Logout()
        {
            teacherID = 0;
            Presentation.DisplayMessage("Logging you out...", Presentation.MessageType.Warning, true);
        }
    }
}