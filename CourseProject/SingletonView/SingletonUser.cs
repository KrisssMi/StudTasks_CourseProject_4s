using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.SingletonView
{
    class SingletonUser
    {
        private static SingletonUser instance;
        public AuthorizationViewModel AuthorizationViewModel { get; set; }
        private SingletonUser(AuthorizationViewModel authorizationView)
        {
            AuthorizationViewModel = authorizationView;
        }
        
        public static SingletonUser getInstance(AuthorizationViewModel viewModel=null)
        {
            return instance ?? (instance = new SingletonUser(viewModel));
        }
    }
}
