using CourseProject.View;
using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.SingletonView
{
    class SingletonAdmin
    {
        private static SingletonAdmin instance;
        public StudentsListViewModel StudentsListViewModel { get; set; }

        private SingletonAdmin(StudentsListViewModel startView)
        {
            StudentsListViewModel = startView;
        }

        public static SingletonAdmin getInstance(StudentsListViewModel startViewModel = null)
        {
            return instance ?? (instance = new SingletonAdmin(startViewModel));
        }
    }
}
