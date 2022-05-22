using CourseProject.DB;
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
    public partial class MessageView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        MessageViewModel messageViewModel;
        EFStudentRepository eFStudent = new EFStudentRepository();

        public MessageView()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            InitializeComponent();
            ConfigurateControlsIfNotAdmin();
            messageViewModel = new MessageViewModel();
            DataContext = messageViewModel;
        }

        public void ClearTextArea()
        {
            Message_Content.Text = "";
        }

        public void SendMessage()
        {
            if (stud.IsAdmin)
            {
                messageViewModel.SendMessage(Message_Content.Text);
                ClearTextArea();
            }
        }

        private void ConfigurateControlsIfNotAdmin()
        {
            if (!stud.IsAdmin)
            {
                Message_Content.Foreground = Brushes.Red;
                Message_Content.Text = "You can't leave messages";
                Message_Content.FontSize = 18;
                Message_Content.TextAlignment = TextAlignment.Center;
                Message_Content.IsReadOnly = true;
                Message_Content.Cursor = Cursors.Arrow;
                //Send_message.Visibility = Visibility.Collapsed;
                Delete_Message.IsEnabled = false;
            }
        }

        private void Delete_Message_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Message m = (Messages.SelectedItem as Message);
                if (m.idStudent == stud.idStudent)
                {
                    messageViewModel.RemoveMessageById(m);
                }
                else MyMessageBox.Show("You can delete only your posts!", MessageBoxButton.OK);
            }
            catch
            {
                MyMessageBox.Show("Choose a message!", MessageBoxButton.OK);
            }
        }


        private void Message_Content_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void Message_Content_PreviewMouseDown(object sender, MouseButtonEventArgs e)   
        {
            ClearTextArea();    
        }
    }
}
