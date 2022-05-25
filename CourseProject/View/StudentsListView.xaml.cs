using CourseProject.ErrorMessage;
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
    public partial class StudentsListView : Page
    {
        Student stud = new Student();
        StudentsListViewModel studentsListViewModel = new StudentsListViewModel();
        public StudentsListView()
        {
            InitializeComponent();
            DataContext = studentsListViewModel;
            Students_Grid.ItemsSource = studentsListViewModel.Students;
        }

        private void Students_Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                studentsListViewModel.UpdateAll();
                Students_Grid.ItemsSource = studentsListViewModel.Students;
                
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var exc in ex.EntityValidationErrors)
                {
                    foreach (var err in (exc as System.Data.Entity.Validation.DbEntityValidationResult).ValidationErrors)
                    {
                        MyMessageBox.Show((err as System.Data.Entity.Validation.DbValidationError).ErrorMessage, MessageBoxButton.OK);
                        break;
                    }
                    break;
                }

            }
            catch (System.InvalidOperationException ex)
            {
                MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
            }
        }

        private void Students_Grid_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (e.AddedCells.Count == 0) return;
                var currentCell = e.AddedCells[0];
                if (currentCell.Column == Students_Grid.Columns[2])
                {
                    // нельзя редактировать айди студента:
                    var cellInfo = currentCell.Column.GetCellContent(currentCell.Item);
                    var textBox = cellInfo.FindName("TextBox") as TextBox;
                    textBox.IsReadOnly = true;
                
                Students_Grid.BeginEdit();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!studentsListViewModel.SelectedItem.IsAdmin)
            {
                studentsListViewModel.RemoveAllInfAboutStudent(studentsListViewModel.SelectedItem);
                Students_Grid.ItemsSource = studentsListViewModel.Students;
            }
            else MyMessageBox.Show("You can't delete the administrator!", MessageBoxButton.OK);
        }
    }
}
