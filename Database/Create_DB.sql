create database StudTasks;
use StudTasks;

create table [User] (
	[idStudent] int not null unique references Student([idStudent]), 
    [Login] nvarchar(50) primary key, 
    [Password] nvarchar(50) not null, 
);

insert into [User] values (10000000, 'admin','admin');
drop table [User];


create table Student (
	[idStudent] int primary key,
	[isAdmin] bit null,
    [Name] nvarchar(20) not null, 
    [Surname] nvarchar(20) not null, 
    [Email] nvarchar(50) null, 
    [Phone] nvarchar(20) null,
    [Description] nvarchar(100) null
);
drop table Student;


create table TimeTable
(
	[idTimeTable] int identity(1, 1) primary key,
	[idStudent] int not null references Student([idStudent]),
	[Day] int,
	[LessonName] nvarchar(50),
	[Auditorium] nvarchar(10),
	[LessonNumber] int,
	[LessonType] nvarchar(2),
	[Week] nvarchar(10),
);
drop table TimeTable;


create table Task
(
	[idTask] int identity(1, 1) primary key,
    [idStudent] int not null references Student([idStudent]),
    [isComplite] bit,
    [DueDate] date null,
	[LessonName] nvarchar(50) null,
    [Importance] int null,
	[Content] nvarchar(50) null,
	[Title] nvarchar(50) null,
);
drop table Task;


create table Subtask
(
	[idSubtask] int identity(1, 1) primary key,
	[idTask] int not null references Task([idTask]),
    [Subtask] nvarchar(100) null,
);
drop table Subtask;


create table Progress
(
	[idProgress] int identity(1, 1) primary key,
	[idStudent] int not null references Student([idStudent]),
	[NeededTasks] int null,
	[ComplitedTasks] int null,
	[TaskProgress] int null,
	[LessonName] nvarchar(50) null
);
drop table Progress;