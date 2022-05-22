using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class TaskListView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        TaskViewModel taskViewModel = new TaskViewModel();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public TaskListView()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            InitializeComponent();
            DataContext = taskViewModel;
        }

        private void ToNewTask_Click(object sender, RoutedEventArgs e)
        {
            ToNewTask.Visibility = Visibility.Hidden;
            NewTask.Visibility = Visibility.Visible;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ToNewTask.Visibility = Visibility.Visible;
            NewTask.Visibility = Visibility.Hidden;
            Clear();
        }




        private void LessonsBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> taskSubjects = new List<string>();
            taskSubjects.AddRange(taskViewModel.GetSubjects());
            LessonsBox.ItemsSource = taskSubjects;
            LessonsBox.SelectedIndex = 0;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Deadline.Text != String.Empty && Title.Text != String.Empty && ImportanceBox.Text != String.Empty)
                {
                    {
                        Model.Task task = new Model.Task { idStudent = stud.idStudent, isComplite = false, LessonName = LessonsBox.SelectedValue.ToString(), DueDate = Convert.ToDateTime(Deadline.SelectedDate), Content = Details.Text, Title = Title.Text, Importance = Convert.ToInt32(ImportanceBox.Text) };
                       
                            taskViewModel.addTask(task);
                            Clear();                       
                    }
                }
                else MyMessageBox.Show("Fill in all the fields", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
            }
        }

        private void Is_complite_Checked(object sender, RoutedEventArgs e)
        {
            taskViewModel.ChangeTrue();
        }

        private void Clear()
        {
            Title.Text = "";
            Details.Text = "";
            LessonsBox.SelectedIndex = 0;
        }

        private void Is_complite_Unchecked(object sender, RoutedEventArgs e)
        {
            taskViewModel.ChangeFalse();
        }

        private void FilterBySubject_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> taskSubjects = new List<string>();
            taskSubjects.AddRange(taskViewModel.GetSubjects());
            taskSubjects.Add("All");
            FilterBySubject.ItemsSource = taskSubjects;
        }

        private void FilterBySubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            taskViewModel.OrderTasks(FilterBySubject.SelectedValue.ToString());
        }

        private void Delete_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (taskViewModel.SelectedTask == null)
            {
                MyMessageBox.Show("Select a task", MessageBoxButton.OK);
            }
            else
            {
                taskViewModel.RemoveTask(taskViewModel.SelectedTask);
            }
        }

        private void TaskComplite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskComplite.SelectedIndex == 0)
            {
                taskViewModel.UpdateFalse();
            }
            else taskViewModel.UpdateTrue();
        }

        private void FilterByImportance_SelectionChanged(object sender, SelectionChangedEventArgs e)                    // поиск по важности
        {
            taskViewModel.OrderByImportance(FilterByImportance.SelectedIndex + 1);
        }
    }
}
