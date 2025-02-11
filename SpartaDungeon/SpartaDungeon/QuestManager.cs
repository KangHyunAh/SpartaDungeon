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
                Console.WriteLine($"[DEBUG] UpdateQuestProgress 호출됨 - 퀘스트 ID: {questId}, 추가 진행량: {count}");
                Console.WriteLine($"[DEBUG] 진행 전 상태: {quest.CurrentProgress} / {quest.GoalCount}");

                quest.CurrentProgress += count;

                if (quest.CurrentProgress >= quest.GoalCount)
                {
                    quest.CurrentProgress =  quest.GoalCount;
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
                    Console.WriteLine($"[DEBUG] 아직 목표 미달성: {quest.CurrentProgress} / {quest.GoalCount}");
                }

            }

            else
            {
                Console.WriteLine($"[DEBUG] 퀘스트 ID {questId}가 존재하지 않음.");
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
            if (quests.TryGetValue(questId, out Quest quest) && !acceptedQuests.Contains(questId))
            {
                acceptedQuests.Add(questId);
                Console.WriteLine($"[퀘스트 수락] {quest.Title}");
                return true;
            }
            return false;
        }

        public bool CompleteQuest(int questId, Player player)
        {
            if (quests.TryGetValue(questId, out Quest quest))
            {
                if (quest.IsCompleted && acceptedQuests.Contains(questId))
                {
                    acceptedQuests.Remove(questId);
                    completedQuests.Add(questId);
                    quest.Status = QuestStatus.Completed;

                    player.gold += quest.RewardGold;
                    player.exp += quest.RewardExp;

                    Console.WriteLine($"[퀘스트 완료] {quest.Title}");
                    Console.WriteLine($"[보상 지급] 골드: {quest.RewardGold}, 경험치: {quest.RewardExp}");

                    return true;
                }
                else
                {
                    Console.WriteLine($"[오류] 완료 조건이 충족되지 않음 (퀘스트 ID: {questId})");
                }
            }
            else
            {
                Console.WriteLine($"[오류] 존재하지 않는 퀘스트 (ID: {questId})");
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
