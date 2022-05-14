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


        private void LoadTimeTableIfEmpty()         // загрузка расписания в пустой таблице
        {
            timeTableViewModel.LoadTT();
        }

        private void UpdateTT(string week)
        {
                timeTableViewModel.GetByWeek(week, (int)stud.idStudent);
                DataContext = new TimeTableViewModel((int)stud.idStudent, week);
        }


        private void Stud_Week_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTT(((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString());
        }

        
        private void Stud_Week_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var el in Stud_Week.Items)                                                             // загружаем все недели в комбобокс
            {
                Stud_Week.SelectedItem = el;
                break;
            }
        }

    }
}
