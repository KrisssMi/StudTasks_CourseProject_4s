create database COURSE_PROJECT;

create table USERS
(
	[UserID] int identity(1, 1) not null primary key, 
    [Login] nvarchar(20) not null, 
    [Password] nvarchar(20) not null, 
    [Name] nvarchar(20) not null, 
    [Surname] nvarchar(20) not null, 
    [Email] nvarchar(50) null, 
    [Phone] nvarchar(20) null,
    [Description] nvarchar(100) null
)


create table TASKS
(
	[TaskID] int identity(1, 1) not null primary key,
    [UserID] int not null,
    [Task] nvarchar(max) not null,
	[Title] nvarchar(50) null,
    [DueDate] date null,
	[LessonName] nvarchar(50) null,
    [Importance] int null,
    [isComplite] bit null,
	foreign key ([UserID]) references USERS([UserID])
)


create table SUBTASKS
(
	[SubtaskID] int identity(1, 1) not null primary key,
	[TaskID] int null,
    [Subtask] nvarchar(max) null,
	foreign key ([TaskID]) references TASKS([TaskID])
)


create table TIMETABLE
(
	[TimeTableID] int identity(1, 1) not null primary key,
	[Day] nvarchar(15) check ([Day] in ('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday')),
	[LessonName] nvarchar(50) null,
	[Auditorium] nvarchar(10) null,
	[LessonNumber] int null,
	[LessonType] nvarchar(5) null,
	[Week] int check ([Week] in (1,2)),
	foreign key ([TimeTableID]) references USERS([UserID])
)


create table PROGRESS
(
	[ProgressID] int identity(1, 1) not null primary key,
	[UserID] int null,
	[NeededTasks] int null,
	[ComplitedTasks] int null,
	[TaskProgress] int null,
	[LessonName] nvarchar(50) null,
	foreign key ([UserID]) references USERS([UserID])
)