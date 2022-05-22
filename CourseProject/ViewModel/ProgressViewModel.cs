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
    class ProgressViewModel
    {

        User user = User.CurrentUser;
        Student stud = new Student();
        EFProgressRepository eFProgress = new EFProgressRepository();       // Репозиторий для прогресса
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();    // Репозиторий для расписания
        EFStudentRepository eFStudent = new EFStudentRepository();          // Репозиторий для студентов
        EFTaskRepository eFTask = new EFTaskRepository();                  // Репозиторий для заданий
        int countProgress = 0;

        Progress selectedItem;
        ObservableCollection<Progress> progresses = new ObservableCollection<Progress>();  // Коллекция прогресса

        public ObservableCollection<Progress> Progresses
        {
            get { return progresses; }
            set { progresses = value; }
        }

        public Progress SelectedItem                                    // Выбранный прогресс
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                    selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public ProgressViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);       // Получаем студента по id
            Update();                                                   // Обновляем прогресс
        }

        public int CountProgress                                            // Количество прогресса
        {
            get { return countProgress; }
            set
            {
                countProgress = value;
                OnPropertyChanged("CountProgress");
            }
        }

        public void addComplitedTasks()         // Добавление завершенных заданий
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.ComplitedTasks < SelectedItem.NeededTasks)
                {
                    SelectedItem.ComplitedTasks += 1;
                    Update();
                }
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void minusComplitedTasks()       // Удаление завершенных заданий
        {
            if (SelectedItem != null)
            {
                SelectedItem.ComplitedTasks -= 1;
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void addNeededTasks()           // Добавление необходимых заданий
        {
            if (SelectedItem != null)
            {
                SelectedItem.NeededTasks += 1;
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void minusNeededTasks()          // Удаление необходимых заданий
        {
            if (SelectedItem != null)
            {
                SelectedItem.NeededTasks -= 1;
                if (SelectedItem.ComplitedTasks > SelectedItem.NeededTasks)
                {
                    SelectedItem.ComplitedTasks -= 1;
                }
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void RemoveById()                // Удаление прогресса по id
        {
            eFProgress.RemoveById(SelectedItem);
            Progresses.Remove(SelectedItem);
        }

        public void OrderProgress()             // Сортировка прогресса
        {
            eFProgress.OrderProgress(stud);
        }

        public IEnumerable<string> GetSubjects()    // Получение предметов
        {
            return eFTimeTable.GetSubjects(stud);   // Получаем предметы по id студента из репозитория расписания
        }

        public void addProgress(Progress progress)  // Добавление прогресса
        {
            Progresses.Add(progress);
            eFProgress.addProgress(progress);
        }

        public Progress GetProgressById(Progress progress)  // Получение прогресса по id
        {
            return eFProgress.GetProgressById(progress);
        }

        public void SaveProgress()
        {
            eFProgress.SaveProgress();
        }

        void Update()                                                       // Обновление прогресса  
        {
            Progresses.Clear();
            foreach (Progress progress in eFProgress.getProgressById(stud)) // Получаем прогресс по id студента
            {
                Progresses.Add(progress);
            }
            if (SelectedItem != null)
            {
                eFProgress.Find(SelectedItem).ComplitedTasks = SelectedItem.ComplitedTasks;             // Обновляем количество завершенных заданий
                eFProgress.Find(SelectedItem).NeededTasks = SelectedItem.NeededTasks;                   // Обновляем количество необходимых заданий
                CountProgress = (int)(SelectedItem.ComplitedTasks * 100 / SelectedItem.NeededTasks);    // Подсчитываем прогресс
                SelectedItem.TaskProgress = CountProgress;                                              // Обновляем прогресс в прогрессе
                eFProgress.Find(SelectedItem).TaskProgress = CountProgress;                             // Обновляем прогресс в базе
                SaveProgress();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;       // Событие изменения свойства
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}