using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProject.View
{
    public partial class ProgressView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        ProgressViewModel progressViewModel = new ProgressViewModel();
        EFStudentRepository eFStudent = new EFStudentRepository();
        EFProgressRepository eFProgress = new EFProgressRepository();

        public ProgressView()
        {
            InitializeComponent();
            stud = eFStudent.GetStudentById((int)user.idStudent);
            DataContext = progressViewModel;
        }

        private void UpdateProgress()
        {
            progressViewModel.OrderProgress();
        }

        private void LessonsBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> progressSubjects = new List<string>();
            progressSubjects.AddRange(progressViewModel.GetSubjects());
            LessonsBox.ItemsSource = progressSubjects;
            LessonsBox.SelectedIndex = 0;
            SetNoElementsNotification();
        }

        private void SetNoElementsNotification()
        {
            if (LessonsBox.Items.Count == 1)
                NotificationMessgeToProgress.Content = "To select the subject must be a shedule :(";
        }

        private void SetExistElementNotification()
        {
            NotificationMessgeToProgress.Content = "You have already selected this item! :)";
        }

        private void ClearNotification()
        {
            NotificationMessgeToProgress.Content = "";
        }

        private bool IsProgressInProgressList(string pr)
        {
            foreach (Progress p in ProgressList.Items)
            {
                if (pr == p.LessonName)
                {
                    SetExistElementNotification();
                    return true;
                }
            }
            ClearNotification();
            return false;
        }

        private void AddProgress_Click(object sender, RoutedEventArgs e)
        {
            ClearNotification();    // очищаем сообщение об ошибке
            if (LessonsBox.SelectedIndex != 0 && !IsProgressInProgressList(LessonsBox.SelectedItem as string))
            {
                try
                {
                    if (eFProgress.getProgressById(stud) == null)
                    {
                        throw new Exception("Нет заданий!");
                    }
                    Progress progress = new Progress { idStudent = stud.idStudent, LessonName = LessonsBox.SelectedValue.ToString(), ComplitedTasks = eFProgress.getProgressById(stud).Where(x => x.LessonName == LessonsBox.SelectedItem as string).First().ComplitedTasks, NeededTasks = eFProgress.getProgressById(stud).Where(x => x.LessonName == LessonsBox.SelectedItem as string).First().NeededTasks, TaskProgress = eFProgress.getProgressById(stud).Where(x => x.LessonName == LessonsBox.SelectedItem as string).First().TaskProgress };
                    progressViewModel.addProgress(progress);
                    progressViewModel.OrderProgress();
                }
                catch (Exception ex)
                {
                    MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
                    Progress progress = new Progress { idStudent = stud.idStudent, LessonName = LessonsBox.SelectedValue.ToString(), ComplitedTasks = 0, NeededTasks = 0, TaskProgress = 0 };
                    progressViewModel.addProgress(progress);
                    progressViewModel.OrderProgress();
                }
            }
        }

        private void NeededTasksMinus_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            progressViewModel.minusNeededTasks();
        }

        private void CompletedTasksPlus_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            progressViewModel.addComplitedTasks();
        }

        private void CompletedTasksMinus_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            progressViewModel.minusComplitedTasks();
        }

        private void NeededTasksPlus_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            progressViewModel.addNeededTasks();
        }
    }
}