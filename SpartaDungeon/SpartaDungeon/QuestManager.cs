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

        public void AddQuest(Quest quest)
        {

            if (!quests.ContainsKey(quest.QuestId))
            {
                quests.Add(quest.QuestId, quest);
                Console.WriteLine($"[퀘스트 추가됨] {quest.Title} 목표: {quest.GoalCount})");
            }

            else
            {
                Console.WriteLine($"[경고] 이미 존재하는 퀘스트 (ID: {quest.QuestId})");
            }
        }

        public void UpdateQuestProgress(int questId, int count, Player player)
        {
            if (quests.TryGetValue(questId, out Quest quest))
            {

                if (quest.CurrentProgress >= quest.GoalCount)
                {
                    quest.CurrentProgress = quest.GoalCount;
                    Console.WriteLine($"[INFO] 퀘스트 '{quest.Title}' 완료!");
                }

                if (quest.Status == QuestStatus.Accepted)
                {
                    quest.UpdateProgress(count);
                    Console.WriteLine($"퀘스트 '{quest.Title}' 진행 상태 업데이트 됨. ({quest.CurrentProgress} / {quest.GoalCount})");
                }

                if (quest.IsCompleted)
                {
                    Console.WriteLine($"[퀘스트 완료 가능] {quest.Title} 퀘스트 목표를 달성했습니다!");
                    CompleteQuest(questId, player);
                }

                else
                {
                    Console.WriteLine($"아직 목표 미달성: {quest.CurrentProgress} / {quest.GoalCount}");
                }

            }

            else
            {
                Console.WriteLine($"퀘스트 ID {questId}가 존재하지 않음.");
            }
        }

        public void ShowAllQuests()
        {
            foreach (var quest in quests.Values)
            {
                string status;
                if (completedQuests.Contains(quest.QuestId))
                    status = "완료";
                else if (acceptedQuests.Contains(quest.QuestId))
                    status = "진행 중";
                else
                    status = "미수락";
                Console.WriteLine($"[{status}] {quest.QuestId}: {quest.Title} - {quest.Description}");
            }
        }

        public bool AcceptQuest(int questId)
        {
            if (quests.TryGetValue(questId, out Quest quest) && !acceptedQuests.Contains(questId) && !quests[questId].IsAccepted && !quests[questId].IsCompleted)
            {
                acceptedQuests.Add(questId);
                quest.Status = QuestStatus.Accepted;
                Console.WriteLine($"[퀘스트 수락] {quest.Title}");
                return true;
            }
            if (quests[questId].IsCompleted)
            {
                Console.WriteLine("이미 완료된 퀘스트 입니다.");
            }
            else if (quests[questId].Status == QuestStatus.Accepted)
            {
                Console.WriteLine("이미 수락된 퀘스트 입니다.");
            }

            return false;
        }

        public bool CompleteQuest(int questId, Player player)
        {
            if (quests.TryGetValue(questId, out Quest quest))
            {

                if (!quest.IsCompleted)
                {
                    Console.WriteLine($"[오류] 퀘스트(ID: {questId})는 아직 완료되지 않았습니다.");
                    return false;
                }

                // 퀘스트가 이미 완료되었을 때 처리
                if (completedQuests.Contains(questId))
                {
                    Console.WriteLine($"[오류] 퀘스트(ID: {questId})는 이미 완료되었습니다.");
                    return false;
                }

                acceptedQuests.Remove(questId);
                completedQuests.Add(questId);

                quest.IsCompleted = true;
                quest.IsAccepted = false;

                player.gold += quest.RewardGold;
                player.exp += quest.RewardExp;

                Console.WriteLine($"[보상 지급] 골드: {quest.RewardGold}, 경험치: {quest.RewardExp}");
                Console.WriteLine($"퀘스트 {quest.Title} 완료 처리됨.");
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

