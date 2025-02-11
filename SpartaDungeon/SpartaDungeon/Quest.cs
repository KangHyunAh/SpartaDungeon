using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Quest
    {
        public int Id {  get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GoalCount { get; }
        public int CurrentCount { get; set;}
        public bool IsCompleted {  get; set; }
        public bool IsAccepted { get; set; }
        public int RewardGold { get; }
        public int RewardExp { get; }

        public Quest(int id, string title, string description, int goalCount, int rewardGold, int rewardExp )
        {
            Id = id;
            Title = title;
            Description = description;
            GoalCount = goalCount;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
            CurrentCount = 0;
            IsCompleted = false;
        }

        

        public void UpdateProgress(Monster monster)
        {
            if(IsCompleted) return;

            CurrentCount++;
            Console.WriteLine($"[진행 중] {Title}: {CurrentCount} / {GoalCount} 처치");
            
            
            if(CurrentCount >= GoalCount)
            {
                IsCompleted = true;
                Console.WriteLine($"[퀘스트 완료] {Title} 퀘스트의 목표를 달성했습니다");
            }
        }

        public void Reward(Player player)
        {
            if (IsCompleted)
            {
                player.gold += RewardGold;
                player.exp += RewardExp;
                Console.WriteLine($"보상: {RewardGold}골드,  {RewardExp} 경험치 지급!");
            }
            else
            {
                Console.WriteLine("퀘스트가 완료되지 않았습니다. 보상을 받을 수 없습니다");
            }
        }
    }
}
