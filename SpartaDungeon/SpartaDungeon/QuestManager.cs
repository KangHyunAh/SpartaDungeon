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

            if (!quests.ContainsKey(quest.Id))
            {
                quests.Add(quest.Id, quest);
                Console.WriteLine($"[퀘스트 추가됨] {quest.Title} (ID: {quest.Id})");
            }

            else
            {
                Console.WriteLine($"[경고] 이미 존재하는 퀘스트 (ID: {quest.Id})");
            }
        }

        public void UpdateQuestProgress(int questId, int progressAmount, Player player)
        {
            if(quests.TryGetValue(questId, out Quest quest))
            {
                if (quest.Status == QuestStatus.Accepted)
                {
                    quest.UpdateProgress(progressAmount);
                    Console.WriteLine($"퀘스트 '{quest.Title}' 진행 상태 업데이트 됨. ({quest.CurrentCount} / {quest.GoalCount})");
                }

                if (quest.IsCompleted)
                {
                    Console.WriteLine($"[퀘스트 완료 가능] {quest.Title} 퀘스트 목표를 달성했습니다!");
                    CompleteQuest(questId, player);
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

    public enum QuestStatus
    {
        Availble,
        Accepted,
        Completed
    }
}
