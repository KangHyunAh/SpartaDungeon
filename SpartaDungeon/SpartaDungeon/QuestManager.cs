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
        private HashSet<int> acceptedQuests = new HashSet<int>();
        private HashSet<int> completedQuests = new HashSet<int>();

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
                string status = acceptedQuests.Contains(quest.Id) ? "진행중" : "미수락";
                Console.WriteLine($"[{status}] {quest.Id}: {quest.Title} - {quest.Description}");
            }
        }

        public bool AcceptQuest(int questId)
        {
            if (quests.TryGetValue(questId, out Quest quest))
            {
                if (!acceptedQuests.Contains(questId))
                {
                    acceptedQuests.Add(questId);
                    Console.WriteLine($"[퀘스트 수락] {quest.Title}");
                    return true;
                }

                else
                {
                    Console.WriteLine("이미 수락한 퀘스트 입니다");
                }
            }
            else
            {
                Console.WriteLine("해당 ID의 퀘스트를 찾을 수 없습니다.");
            }
            return false;
        }
    }
}
