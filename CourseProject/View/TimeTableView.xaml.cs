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
    /// Логика взаимодействия для TimeTableView.xaml
    /// </summary>
    public partial class TimeTableView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        TimeTableViewModel timeTableViewModel;
        EFStudentRepository eFStudent = new EFStudentRepository();

        public TimeTableView()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);

            InitializeComponent();
            timeTableViewModel = new TimeTableViewModel();
        }


        private void LoadGroupsId()
        {
            if (stud.IsAdmin)
            {
                Choose_idStudent.Visibility = Visibility.Visible;
            }
            else
            {
                Choose_idStudent.Visibility = Visibility.Hidden;
            }
        }

        private void LoadTimeTableIfEmpty()
        {
            timeTableViewModel.LoadTT();
        }

        private void UpdateTT(string week)
        {
            if (stud.IsAdmin)
            {
                int idStudent = Convert.ToInt32(Choose_idStudent.SelectedValue);
                timeTableViewModel.GetByWeekAdmin(week, idStudent);
                DataContext = new TimeTableViewModel(idStudent, week);
            }

            else
            {
                timeTableViewModel.GetByWeek(week, (int)stud.idStudent);
                DataContext = new TimeTableViewModel((int)stud.idStudent, week);
            }
        }


        private void Stud_Week_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTT(((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString());
        }

        private void Stud_Week_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var el in Stud_Week.Items)
            {
                if ((el is ComboBoxItem))
                {
                    if ((el as ComboBoxItem).Content.ToString() == timeTableViewModel.CurrentWeek())
                    {
                        (el as ComboBoxItem).IsSelected = true;
                        break;
                    }
                }
            }
        }

    }
}
