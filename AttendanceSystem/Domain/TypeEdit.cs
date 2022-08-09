using System;

namespace Domain
{
    public partial class Course
    {
        public Course(string courseTitle)
        {
            this.CourseTitle = courseTitle;
        }
    }

    public partial class Group
    {
        public Group(string name, int courseID)
        {
            this.Name = name;
            this.CourseID = courseID;
        }
    }

    public partial class Lesson
    {
        public Lesson(int groupID, DateTime dateTime, int teacherID)
        {
            this.GroupID = groupID;
            this.DateTime = dateTime;
            this.TeacherID = teacherID;
        }
    }

    public partial class Student
    {
        public Student(string name, string surname, string email, int groupID)
        {
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.GroupID = groupID;
        }
    }

    public partial class StudentAttendance 
    {
        public StudentAttendance() {}

        public StudentAttendance(int lessonID, bool presence, int studentID)
        {
            this.LessonID = lessonID;
            this.Presence = presence;
            this.StudentID = studentID;
        }
    }

    public partial class Teacher
    {
        public Teacher(string username, string password, string name, string surname, string email)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
        }
    }
}