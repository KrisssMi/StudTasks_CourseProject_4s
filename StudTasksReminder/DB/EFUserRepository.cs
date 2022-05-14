using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFUserRepository
    {
        private StudTasksEntities context;     // контекст базы данных

        public EFUserRepository()
        {
            context = new StudTasksEntities(); // инициализация контекста
        }

        public EFUserRepository(StudTasksEntities c)
        {
            context = c;
        }

        public IEnumerable<User> getUsers()    // получение всех пользователей из коллекции
        {
            return context.User;
        }

        public void addUser(User user)         // добавление пользователя в коллекцию
        {
            context.User.Add(user);
            context.SaveChanges();             // сохранение изменений в базе данных
        }

        public void Update(User user)          // обновление пользователя в коллекции
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Save()                     // сохранение изменений в базе данных
        {
            context.SaveChanges();
        }

        public void Remove(User user)         // удаление пользователя из коллекции
        {
            context.User.Remove(user);
            context.SaveChanges();
        }

        public User GetUserById(int id)       // получение пользователя по его id
        {
            return context.User.FirstOrDefault(x => x.idStudent == id);
        }

        public void RemoveUserById(Student student)     // удаление пользователя по его id
        { 
            var user = context.User.FirstOrDefault(x => x.idStudent == student.idStudent);
            if (user!=null)
            {
                context.User.Remove(GetUserById(student.idStudent));
            }
            context.SaveChanges();
        }

        public User GetUserByLogin(string login)        // получение пользователя по его логину
        {
            return context.User.FirstOrDefault(x => x.Login == login);
        }
    }
}
