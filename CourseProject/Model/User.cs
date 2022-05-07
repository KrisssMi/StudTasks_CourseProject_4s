//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseProject.Model
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows;

    public partial class User
    {
        public Nullable<int> idStudent { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Student Student { get; set; }

        public User()
        {
        }

        public User(int idstudent, string login, string password)
        {
            idStudent = idstudent;
            Login = login;
            Password = password;
        }

        private static User currentUser;

        public static User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (value != null)          // если пользователь не авторизован,
                    currentUser = value;    // то присваиваем ему значение пользователя из БД
                else
                    MessageBox.Show("Ошибка входа!");
            }
        }

        
        // получение хэш-кода пароля
        //public static string getHash(string password)
        //{
        //    if (String.IsNullOrEmpty(password))
        //    {
        //        return "-1";
        //    }
        //    else
        //    {
        //        var md5 = MD5.Create();
        //        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return Convert.ToBase64String(hash);
        //    }
        //}
    }
}
