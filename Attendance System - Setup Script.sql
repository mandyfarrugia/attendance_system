create database attendance_system;
go

create table dbo.Teacher (
	TeacherID integer
		constraint te_tid_pk primary key,
	Username nvarchar(50) not null,
	[Password] nvarchar(50) not null,
	[Name] nvarchar(50) not null,
	Surname nvarchar(50) not null,
	Email nvarchar(50) not null
);

create table dbo.Course (
	CourseID integer
		constraint co_cid_pk primary key,
	Course nvarchar(50) not null
);

create table dbo.[Group] (
	GroupID integer
		constraint gr_gid_pk primary key,
	[Name] nvarchar(50) not null,
	CourseID integer not null
		constraint gr_cid_fk foreign key references dbo.Course(CourseID)
);

create table dbo.Student (
	StudentID integer
		constraint st_sid_pk primary key,
	[Name] nvarchar(50) not null,
	Surname nvarchar(50) not null,
	Email nvarchar(50) not null,
	GroupID integer not null
		constraint st_gid_fk foreign key references dbo.[Group](GroupID)
);

create table dbo.Lesson (
	LessonID integer
		constraint le_lid_pk primary key,
	GroupID integer not null
		constraint le_gid_fk foreign key references dbo.[Group](GroupID),
	[DateTime] datetime not null,
	TeacherID int not null
		constraint le_tid_fk foreign key references dbo.Teacher(TeacherID)
);

create table dbo.StudentAttendance (
	AttendanceID integer,
	LessonID integer
		constraint sa_lid_fk foreign key references dbo.Lesson(LessonID),
	Presence bit not null,
	StudentID integer
		constraint sa_sid_fk foreign key references dbo.Student(StudentID),
	constraint sa_pk primary key(AttendanceID, LessonID, StudentID)
);