using CourseProject.DB;
using CourseProject.Model;
using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProject.View
{
    /// <summary>
    /// Логика взаимодействия для ProgressView.xaml
    /// </summary>
    public partial class ProgressView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        ProgressViewModel progressViewModel = new ProgressViewModel();
        EFStudentRepository eFStudent = new EFStudentRepository();
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            progressViewModel.RemoveById();
        }

        private void LessonsBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> progressSubjects = new List<string> { "Choose subject" };
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
            NotificationMessgeToProgress.Content = "U have already selected this item! :)";
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
            ClearNotification();
            if (LessonsBox.SelectedIndex != 0 && !IsProgressInProgressList(LessonsBox.SelectedItem as string))
            {
                Progress progress = new Progress { ComplitedTasks = 0, idStudent = stud.idStudent, LessonName = LessonsBox.SelectedValue.ToString(), NeededTasks = 1, TaskProgress = 0 };
                progressViewModel.addProgress(progress);
                progressViewModel.OrderProgress();
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
