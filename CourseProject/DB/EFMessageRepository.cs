using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFMessageRepository
    {
        User user = User.CurrentUser;

        private StudTasksEntities context;

        public EFMessageRepository()
        {
            context = new StudTasksEntities();
        }

        public EFMessageRepository(StudTasksEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Message> getMessages()
        {
            return context.Message;
        }

        public int CountMessages()
        {
            return context.Message.Count();
        }

        public void addMessage(Message message)
        {
            context.Message.Add(message);
            context.SaveChanges();
        }

        public void Remove(Message message)
        {
            context.Message.Remove(message);
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)
        {
            if (student != null)
            {
                var messages = context.Message.Where(m => m.idStudent == student.idStudent);
                foreach (var message in messages)
                {
                    context.Message.Remove(message);
                }
                context.SaveChanges();
            }
        }

        public Message GetMessageById(int id)
        {
            return context.Message.FirstOrDefault(x => x.idMessage == id);
        }

        public void OrderMessage()
        {
            context.Message.Distinct().OrderByDescending(p => p.DateOfCreate).Load();
        }

        public void RemoveMessageById(Message message)
        {
            context.Message.Remove(GetMessageById(message.idMessage));
            context.SaveChanges();
        }
    }
}
