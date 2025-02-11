using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class QuestManager
    {
        public Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
        public HashSet<int> acceptedQuests = new HashSet<int>();
        public HashSet<int> completedQuests = new HashSet<int>();

        public QuestManager() { }

        public void AddQuest( Quest quest)
        {
            quests[quest.Id] = quest;

        }

        public void CompleteQuest(int questId)
        {
            if(quests.TryGetValue(questId, out Quest quest))
            {
                quest.CompleteQuest();
            }

        }

        public void ShowAllQuests()
        {
            foreach (var quest in quests.Values)
            {
                string status;
                if (completedQuests.Contains(quest.Id))
                    status = "완료";
                else if (acceptedQuests.Contains(quest.Id))
                    status = "진행 중";
                else
                    status = "미수락";
                Console.WriteLine($"[{status}] {quest.Id}: {quest.Title} - {quest.Description}");
            }
        }

        public bool AcceptQuest(int questId)
        {
            if (quests.TryGetValue(questId, out Quest quest) && !acceptedQuests.Contains(questId))
            {
                acceptedQuests.Add(questId);
                Console.WriteLine($"[퀘스트 수락] {quest.Title}");
                return true;
            }
            return false;
        }
    }
}
