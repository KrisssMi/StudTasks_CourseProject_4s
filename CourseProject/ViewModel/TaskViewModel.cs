﻿using CourseProject.DB;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.ViewModel
{
    class TaskViewModel : INotifyPropertyChanged
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFTaskRepository eFTaskRepository = new EFTaskRepository();
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        private Model.Task selectedTask;

        private ObservableCollection<Model.Task> unsatisfiedTasks = new ObservableCollection<Model.Task>();
        private ObservableCollection<Model.Subtask> unsatisfiedSubtasks = new ObservableCollection<Model.Subtask>();


        public ObservableCollection<Model.Task> UnsatisfiedTasks    // невыполненные задания
        {
            get { return unsatisfiedTasks; }
            set
            {
                unsatisfiedTasks = value;
                OnPropertyChanged("Tasks");
            }
        }


        public ObservableCollection<Model.Subtask> UnsatisfiedSubtasks    // невыполненные задания
        {
            get { return unsatisfiedSubtasks; }
            set
            {
                unsatisfiedSubtasks = value;
                OnPropertyChanged("Subtasks");
            }
        }
        

        public Model.Task SelectedTask                              // выбранное задание
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                OnPropertyChanged("SelectedTask");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public TaskViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);       // получаем студента по id
            //eFTimeTable.Clear();
        }

        public void ChangeTrue()        // изменение выполненности задания
        {
            if (SelectedTask != null)
            {
                SelectedTask.isComplite = true;
                eFTaskRepository.ChangeComplite(SelectedTask);
                UpdateFalse();
            }
        }


        public void ChangeFalse()
        {
            if (SelectedTask != null)
            {
                SelectedTask.isComplite = false;
                eFTaskRepository.ChangeComplite(SelectedTask);
                UpdateTrue();
            }
        }

        public void UpdateFalse()           // обновление невыполненных заданий
        {
            UnsatisfiedTasks.Clear();

            foreach (Model.Task t in eFTaskRepository.GetTasksById(stud))
            {
                if (t.isComplite == false)
                {
                    UnsatisfiedTasks.Add(t);
                }
            }
        }

        public void UpdateTrue()
        {
            UnsatisfiedTasks.Clear();

            foreach (Model.Task t in eFTaskRepository.GetTasksById(stud))
            {
                if (t.isComplite == true)
                {
                    UnsatisfiedTasks.Add(t);
                }
            }
        }

        //public IEnumerable<string> GetSubjects()
        //{
        //    return eFTimeTable.GetSubjects(stud);
        //}

        public void addTask(Model.Task task)
        {
            eFTaskRepository.addTask(task);
            UnsatisfiedTasks.Add(task);
        }

        public void addSubtask(Model.Subtask subtask)
        {
            eFTaskRepository.addSubtask(subtask);
            UnsatisfiedSubtasks.Add(subtask);
        }

        public void RemoveTask()
        {
            eFTaskRepository.RemoveById(SelectedTask);
            UnsatisfiedTasks.Remove(SelectedTask);
        }

        public void OrderTasks(string subject)
        {
            UnsatisfiedTasks.Clear();
            if (subject.Equals("All"))
            {
                foreach (Model.Task task in eFTaskRepository.GetTasksById(stud))
                    UnsatisfiedTasks.Add(task);
            }
            else
            {
                foreach (Model.Task task in eFTaskRepository.getEnum(stud, subject))
                    UnsatisfiedTasks.Add(task);
            }
        }
    }
}
