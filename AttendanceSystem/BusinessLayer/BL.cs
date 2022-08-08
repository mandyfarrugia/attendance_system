using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Domain;

namespace BusinessLayer
{
    public class BL
    {
        static DL dataLayer = new DL();

        public bool VerifyIfTeacherUsernameExists(string username)
        {
            Teacher matchingTeacher = dataLayer.VerifyIfTeacherUsernameExists(username);
            return matchingTeacher != null;
        }

        public bool VerifyIfTeacherEmailExists(string email)
        {
            Teacher matchingTeacher = dataLayer.VerifyIfTeacherEmailExists(email);
            return matchingTeacher != null;
        }

        public bool VerifyIfTeacherPasswordIsCorrect(string username, string password)
        {
            bool credentialsMatch = dataLayer.VerifyIfTeacherPasswordIsCorrect(username, password);
            return credentialsMatch;
        }

        public int GetTeacherID(string username, string password)
        {
            int loggedInTeacherID = dataLayer.GetTeacherID(username, password);
            return loggedInTeacherID;
        }

        public bool VerifyIfStudentEmailExists(string email)
        {
            Student matchingStudent = dataLayer.VerifyIfStudentEmailExists(email);
            return matchingStudent != null;
        }

        public void AddNewGroup(string groupName, int courseID)
        {
            Group group = new Group(groupName, courseID);
            dataLayer.AddNewGroup(group);
        }

        public List<Group> FetchAllGroups()
        {
            return dataLayer.FetchAllGroups();
        }

        public bool CheckIfCourseExistsById(int courseID)
        {
            Course course = dataLayer.CheckIfCourseExistsById(courseID);
            return course != null;
        }

        public List<Course> FetchAllCourses()
        {
            return dataLayer.FetchAllCourses();
        }

        public void AddNewCourse(string courseTitle)
        {
            Course course = new Course(courseTitle);
            dataLayer.AddNewCourse(course);
        }

        public void AddNewTeacher(string username, string password, string name, string surname, string email)
        {
            Teacher teacher = new Teacher(username, password, name, surname, email);
            dataLayer.AddNewTeacher(teacher);
        }

        public void AddNewStudent(string name, string surname, string email, int groupID)
        {
            Student student = new Student(name, surname, email, groupID);
            dataLayer.AddNewStudent(student);
        }

        public string EditTeacher(int teacherID, string newUsername, string newPassword, string newName, string newSurname, string newEmail)
        {
            string updates = dataLayer.EditTeacher(teacherID, newUsername, newPassword, newName, newSurname, newEmail);
            return updates;
        }
    }
}