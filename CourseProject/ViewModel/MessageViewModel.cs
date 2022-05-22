using CourseProject.DB;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.ViewModel
{
    class MessageViewModel : INotifyPropertyChanged
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFMessageRepository eFMessage = new EFMessageRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        ObservableCollection<Message> tmpMessages = new ObservableCollection<Message>();


        public ObservableCollection<Message> Messages
        {
            get { return tmpMessages; }
        }

        public MessageViewModel()
        {
            stud = eFStudent.GetStudentById(stud.idStudent);
            var messages = this.getMessages();
            tmpMessages.Clear();
            foreach (Message m in messages)
                tmpMessages.Add(m);
        }

        public IEnumerable<Message> getMessages()
        {
            eFMessage.OrderMessage();
            return eFMessage.getMessages();
        }

        public void RemoveMessageById(Message message)
        {
            eFMessage.RemoveMessageById(message);
            tmpMessages.Remove(message);
        }

        public void SendMessage(string content)
        {
            if (!content.Equals(String.Empty))
            {
                Message m = new Message(user.idStudent, content);
                eFMessage.addMessage(m);
                tmpMessages.Add(m);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
