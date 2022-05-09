using CourseProject.DB;
using CourseProject.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace CourseProject.View
{
    /// <summary>
    /// Логика взаимодействия для ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFStudentRepository eFStudent = new EFStudentRepository();
        
        public ProfileView()
        {
            InitializeComponent();
            FillInfo();

        }

        private void FillInfo()
        {
            UserName.Content = stud.Name + " " + stud.Surname;
            lbl_Email.Text = stud.Email;
            lbl_Phone.Text = stud.Phone;
        }

        private byte[] _imageBytes = null;
        private void Btn_UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
            };
            if (dialog.ShowDialog() != true) { return; }

            lbl_PhotoPath.Content = dialog.FileName;
            img_ProfilePhoto.ImageSource = new BitmapImage(new Uri(lbl_PhotoPath.Content.ToString()));

            using (var fs = new FileStream(lbl_PhotoPath.Content.ToString(), FileMode.Open, FileAccess.Read))
            {
                _imageBytes = new byte[fs.Length];
                fs.Read(_imageBytes, 0, System.Convert.ToInt32(fs.Length));
            }
            //SaveImage();
        }


        //private void SaveImage()
        //{
        //    if (!String.IsNullOrEmpty(lbl_PhotoPath.Content.ToString()))
        //    {
        //        attach.ImagePath = lbl_PhotoPath.Content.ToString();
        //        attach.Image = _imageBytes;
        //        Registration.unit.Attachments.Update(attach);
        //        Registration.unit.Save();
        //    }
        //}

        //private void LoadImage()
        //{
        //    try
        //    {
        //        if (attach != null && attach.Image != null && attach.ImagePath != null)
        //        {
        //            img_ProfilePhoto.ImageSource = new BitmapImage(new Uri(attach.ImagePath));
        //            lbl_PhotoPath.Content = attach.ImagePath;
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Photo path " + attach.ImagePath + " is not found");
        //    }
        //}



    }
}
