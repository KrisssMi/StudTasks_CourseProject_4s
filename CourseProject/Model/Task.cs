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
    
    public partial class Task
    {
        public int idTask { get; set; }
        public int idStudent { get; set; }
        public Nullable<bool> isComplite { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string LessonName { get; set; }
        public Nullable<int> Importance { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    
        public virtual Student Student { get; set; }

        public string Date
        {
            get { return DueDate.Value.Day + "/" + DueDate.Value.Month + "/" + DueDate.Value.Year; }
        }        
    }
}
