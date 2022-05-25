using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.ViewModel
{
    class TaskViewModel : BaseViewModel
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFTaskRepository eFTaskRepository = new EFTaskRepository();
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();
        EFProgressRepository eFProgress = new EFProgressRepository();

        private Model.Task selectedTask;

        private ObservableCollection<Model.Task> unsatisfiedTasks = new ObservableCollection<Model.Task>();


        public ObservableCollection<Model.Task> UnsatisfiedTasks        // невыполненные задания
        {
            get { return unsatisfiedTasks; }
            set
            {
                unsatisfiedTasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        public Model.Task SelectedTask                                  // выбранное задание
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                OnPropertyChanged("SelectedTask");
            }
        }

        public IEnumerable<string> GetSubjects()                        // получение предметов из таблицы расписания
        {
            return eFTimeTable.GetSubjects(stud);
        }
        

        public TaskViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);       // получаем студента по id
            eFTimeTable.Clear();
        }

        public void ChangeTrue()                                        // изменение выполненности задания
        {
            if (SelectedTask != null)
            {
                SelectedTask.isComplite = true;
                var currentProgress = eFProgress.getProgress().Where(x => x.idStudent == stud.idStudent && x.LessonName == SelectedTask.LessonName).First();
                currentProgress.ComplitedTasks++;
                currentProgress.TaskProgress = (int)(currentProgress.ComplitedTasks * 100 / currentProgress.NeededTasks);
                eFProgress.Update(currentProgress);
                eFTaskRepository.ChangeComplite(SelectedTask);
                UpdateFalse();
            }
        }

        public void ChangeFalse()
        {
            if (SelectedTask != null)
            {
                SelectedTask.isComplite = false;
                var currentProgress = eFProgress.getProgress().Where(x => x.idStudent == stud.idStudent && x.LessonName == SelectedTask.LessonName).First();
                currentProgress.ComplitedTasks--;
                currentProgress.TaskProgress = (int)(currentProgress.ComplitedTasks * 100 / currentProgress.NeededTasks);
                eFProgress.Update(currentProgress);
                eFTaskRepository.ChangeComplite(SelectedTask);
                UpdateTrue();
            }
        }

        public void UpdateFalse()                                       // обновление невыполненных заданий
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


        public void addTask(Model.Task task)
        {
            Progress currentProgress;
            try
            {
                eFTaskRepository.addTask(task);
                currentProgress = eFProgress.getProgress().Where(x => x.idStudent == stud.idStudent && x.LessonName == task.LessonName).First();
                UnsatisfiedTasks.Add(task);
            }
            catch (Exception ex)
            {
                eFProgress.addProgress(new Progress() { idStudent = stud.idStudent, LessonName = task.LessonName, ComplitedTasks = 0, NeededTasks = 0, TaskProgress = 0 });
                eFProgress.SaveProgress();
                currentProgress = eFProgress.getProgress().Where(x => x.idStudent == stud.idStudent && x.LessonName == task.LessonName).First();
                UnsatisfiedTasks.Add(task);
            }
            currentProgress.NeededTasks++;
            currentProgress.TaskProgress = (int)(currentProgress.ComplitedTasks * 100 / currentProgress.NeededTasks);
            eFProgress.Update(currentProgress);          
        }

        public void RemoveTask(Model.Task selectedTask)
        {
            try
            {
                Progress currentProgress;
                eFTaskRepository.RemoveById(SelectedTask);
                UnsatisfiedTasks.Remove(SelectedTask);
                currentProgress = eFProgress.getProgress().Where(x => x.idStudent == stud.idStudent && x.LessonName == selectedTask.LessonName).First();
                if ((bool)selectedTask.isComplite == true)
                {
                    currentProgress.ComplitedTasks--;
                    currentProgress.NeededTasks--;
                }
                else
                {
                    currentProgress.NeededTasks--;
                }
                try
                {
                    currentProgress.TaskProgress = (int)(currentProgress.ComplitedTasks * 100 / currentProgress.NeededTasks);
                    eFProgress.Update(currentProgress);
                }
                catch (Exception)
                {
                    eFProgress.Remove(currentProgress);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
            }
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


        public void OrderByImportance(int imp)
        {
            UnsatisfiedTasks.Clear();
            if (imp.Equals(1))
            {
                foreach (Model.Task task in eFTaskRepository.getEnumByImportance(stud, imp))
                    UnsatisfiedTasks.Add(task);
            }
            else if (imp.Equals(2))
            {
                foreach (Model.Task task in eFTaskRepository.getEnumByImportance(stud, imp))
                    UnsatisfiedTasks.Add(task);
            }
            else if (imp.Equals(3))
            {
                foreach (Model.Task task in eFTaskRepository.getEnumByImportance(stud, imp))
                    UnsatisfiedTasks.Add(task);
            }
        }
    }
}
