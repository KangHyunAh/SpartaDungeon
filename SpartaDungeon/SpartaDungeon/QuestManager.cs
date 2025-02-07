using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class QuestManager
    {
        private Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
        private List<int> acceptedQuests = new List<int>();

        public void AddQuest( Quest quest)
        {
            if (!quests.ContainsKey(quest.Id))
            {
                quests.Add(quest.Id, quest);
                Console.WriteLine("$퀘스트 '{quest.Title}'추가됨.");
            }
        }

        public void CompleteQuest(int questId)
        {
            if(quests.TryGetValue(questId, out Quest quest))
            {
                quest.CompleteQuest();
            }
            else
            {
                Console.WriteLine("해당 ID의 퀘스트를 찾을 수 없음");
            }
        }

        public void ShowAllQuests()
        {
            foreach (var quest in quests.Values)
            {
                Console.WriteLine($"[{(quest.IsCompleted ? "완료" : "진행 중")}] {quest.Title} - {quest.Description}");
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
