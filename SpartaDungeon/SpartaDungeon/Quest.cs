using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Quest
    {
        public int Id {  get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted {  get; private set; }

        public Quest(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
            IsCompleted = false;
        }

        public void CompleteQuest()
        {
            IsCompleted = true;
            Console.WriteLine($"퀘스트'{Title}' 완료!");
        }
    }
}
