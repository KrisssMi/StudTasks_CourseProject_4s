using StudTasksReminder.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFStudentRepository
    {
        private StudTasksEntities context;              // контекст базы данных

        public EFStudentRepository()
        {
            context = new StudTasksEntities();          // инициализация контекста
        }

        public EFStudentRepository(StudTasksEntities c) // инициализация контекста
        {
            context = c;
        }

        public IEnumerable<Student> getStudents()       // получение списка студентов
        {
            return context.Student;
        }

        public void addStudent(Student student)         // добавление студента
        {
            context.Student.Add(student);
            context.SaveChanges();                      // сохранение изменений в базе данных
        }

        public void Update(Student student)             // обновление студента
        {
            context.Entry(student).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateAll()                         // обновление всех студентов
        {
            context.SaveChanges();
        }

        public void Remove(Student student)             // удаление студента    
        {
            context.Student.Remove(student);
            context.SaveChanges();
        }

        public Student GetStudentById(int id)           // получение студента по его идентификатору
        {
            return context.Student.FirstOrDefault(x => x.idStudent == id);
        }

        
        public void RemoveStudentById(Student student)  // удаление студента по его идентификатору
        {
        var stud = context.Student.FirstOrDefault(x => x.idStudent == student.idStudent);
            if (stud != null)
            {
                context.Student.Remove(GetStudentById(student.idStudent));
            }
            context.SaveChanges();
        }


        public IEnumerable<Student> getStudentsNoAdmin()
        {
            return context.Student.Where(s => s.isAdmin == false);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
