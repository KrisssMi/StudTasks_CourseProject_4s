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
    using CourseProject.ErrorMessage;
    using System;

    public partial class Message
    {
        public int idMessage { get; set; }
        public int idStudent { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> DateOfCreate { get; set; }
    
        public virtual Student Student { get; set; }

        public Message() { }

        public Message(int idstudent, string content)
        {
            idStudent = idstudent;
            Content = content;
            DateOfCreate = DateTime.Now;
        }
    }
}