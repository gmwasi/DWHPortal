using System.Collections.Generic;
using System.Text;

namespace DWHDashboard.SharedKernel.Model
{
    public class SyncSummary<T>

    {
        public string Subject { get; set; }
        public int Inserts { get; set; }
        public List<T> InsertList { get; set; } = new List<T>();
        public int Updates { get; set; }
        public List<T> UpdateList { get; set; } = new List<T>();
        public int Voids { get; set; }
        public List<T> VoidsList { get; set; } = new List<T>();

        public SyncSummary(string subject)
        {
            Subject = subject;
        }

        public void AddInsertList(List<T> list)
        {
            InsertList.AddRange(list);
            Inserts = InsertList.Count;
        }

        public void AddUpdateList(List<T> list)
        {
            UpdateList.AddRange(list);
            Updates = UpdateList.Count;
        }

        public void AddVoidList(List<T> list)
        {
            VoidsList.AddRange(list);
            Voids = VoidsList.Count;
        }

        public override string ToString()
        {
            return $"{Subject} synchronized,  ({Inserts}) New, ({Updates}) Updated, ({Voids}) Removed ";
        }

        public string ShowSummary()
        {
            var message = new StringBuilder();
            message.AppendLine(ToString());
            if (Inserts > 0)
            {
                message.AppendLine("NEW");
                message.AppendLine(new string('-', 40));
                int count = 0;
                foreach (var item in InsertList)
                {
                    count++;
                    message.AppendLine($"  {count}.{item}");
                }
            }

            if (Updates > 0)
            {
                message.AppendLine("UPDATED");
                message.AppendLine(new string('-', 40));
                int count = 0;
                foreach (var item in UpdateList)
                {
                    count++;
                    message.AppendLine($"  {count}.{item}");
                }
            }

            if (Voids > 0)
            {
                message.AppendLine("REMOVED");
                message.AppendLine(new string('-', 40));
                int count = 0;
                foreach (var item in VoidsList)
                {
                    count++;
                    message.AppendLine($"  {count}.{item}");
                }
            }
            return message.ToString();
        }
    }
}